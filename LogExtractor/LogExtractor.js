var xml2js = require('xml2js'),
    fs = require('graceful-fs'),
    _ = require('lodash'),
    util = require('util');

var LOG_DIR = "./Output";
var ParsedLogs = new Array();
var parser = new xml2js.Parser();



var daisy_dirs = fs.readdirSync(LOG_DIR);
for ( var dirIndex = 0; dirIndex < daisy_dirs.length; dirIndex++) {
    var daisy_dir = daisy_dirs[dirIndex];

    var files = fs.readdirSync(LOG_DIR + "/" + daisy_dir);
    for ( var fileIndex = 0; fileIndex < files.length; fileIndex++) {
        if (files[fileIndex].match(/^LOG-.+Migration.xml$/)) {
            ParsedLogs.push({
                'daisyInput' : daisy_dir,
                'filePath' : LOG_DIR + "/" + daisy_dir + "/" + files[fileIndex],
                'errors' : new Array()
            });
        }
    }
}

var remaining = ParsedLogs.length;

console.log("<log>");
ParsedLogs.forEach(function(parseItem) {

    var file_callback = _.bind(function(file_err, data) {

        var localParseItem = this.parseItem;

        //Handle Errors
        if (file_err) {
            if (file_err.errno === 20) {
                console.log('No File Handles') //Could not open, so we can try again later
                return; //Exit this callback so remaining is not decremented
            }

            console.log("File Read Error");
            console.dir(file_err);

            //Parse Error or not, decrement remaining
            //As this is the "deepest aschynchronus callback
            remaining -= 1;

            if (remaining == 0) {
                console.log("Done from Parse: ");
                handleParsedLogs();
            }

        } else {
            parser.parseString(data, function(parse_err, result) {
                if (parse_err) {
                    //The last xml file seems to trip this for some reason...
                    //...but only on some environments
                    console.dir(parse_err);
                    console.dir(this.parseItem.filePath);
                    console.log(data);
                    console.log("Parse Error")

                    //No decrementing count??
                } else {
                    //Extract item
                    parseLog(localParseItem, result);

                    //Parse Error or not, decrement remaining
                    //As this is the "deepest aschynchronus callback
                    remaining -= 1;

                    if (remaining == 0) {
                        handleParsedLogs();
                    }

                }
            });
            //Do work here
        }


    }, { parseItem : parseItem });

    fs.readFile(parseItem.filePath, file_callback);
});

function parseLog(parseItem, result) {
    for(var taskIndex = 0; taskIndex < result.log.task.length; taskIndex++) {
        var task = result.log.task[taskIndex];

        if (task.item && task.item.length > 0) {
            for (var itemIndex = 0; itemIndex < task.item.length; itemIndex++) {
                var item = task.item[itemIndex];

                if (item.error && item.error.length > 0) {
                    var error = item.error[0].message[0];
                    var migID = "";
                    var type = "";

                    if (item.mig_item && item.mig_item[0].FullItemDescription) {
                        migID = item.mig_item[0].FullItemDescription[0].properties[0].UniqueIdentifier[0].$.value;
                        type = "Content";
                    } else if (item.mig_item && item.mig_item[0].RelationshipDescription) {
                        migID = item.mig_item[0].RelationshipDescription[0].properties[0].OwnerUniqueIdentifier[0].$.value;
                        type = "Relationship";
                    } else {
                        migID = "unknown";
                        type = "unknown";
                    }

                    console.log(
                        "\t<entry>\n" +
                        "\t\t<file>" + parseItem.daisyInput + "</file>\n" +
                        "\t\t<mig_id>" + migID + "</mig_id>\n" +
                        "\t\t<sheet>" + type + "</sheet>\n" +
                        "\t\t<error><![CDATA[" + error + "]]></error>\n" +
                        "</entry>"
                    );
                }
            }
        }
    }
}


function handleParsedLogs() {
    console.log("</log>");
}



/*
//Loop through daisy input folders
fs.readdir(LOG_DIR, function(dir_err, daisy_dirs) {

    for ( var dirIndex = 0; dirIndex < daisy_dirs.length; dirIndex++) {
        var daisy_dir = daisy_dir[dirIndex];

        //Loop through the files in each folder
        fs.readdir(LOG_DIR + "/" + daisy_dir, function(dir_err2, files) {
            for ( var fileIndex = 0; fileIndex < files.length; fileIndex++) {

                //Only handle the file if it does not match a Log Input.
                //NOTE: We assume only 1 file matches.
                if (files[fileIndex].match(/^LOG-.+Migration.xml$/)) {
                    var log_file = files[fileIndex];

                    console.log(daisy_dir + ":" + files[fileIndex]);

                    var filepath = LOG_DIR + "/" + daisy_dir + "/" + log_file;

                    //Define the callback for the file load.
                    var file_callback = _.bind(function(file_err, data) {
                        var local_dir = this.dir;

                        if (file_err) {
                            if (file_err.errno === 20) {
                                console.log('No File Handles')
                            } else {
                                console.log("File Read Error");
                                console.dir(file_err);

                                //We never made it to parse for the final item, so this
                                //is the "deepest aschynchronus callback
                                remaining -= 1;

                                if (remaining == 0) {
                                    console.log("Done from file error " + trials.length);
                                    drugList = buildDrugListFromTrials(trials);
                                    handleTrials(trials, drugList);
                                }

                            }
                        } else {
                            //do work
                        }
                    }, {'dir': daisy_dir});

                    fs.readFile(filepath, file_callback);
                }
            }
        });
    }
});


function handleDaisyFolder(daisy_dir) {
}

function handleDaisyLog(daisy_dir, log_file) {
}

*/