var xml2js = require('xml2js'),
    fs = require('graceful-fs'),
    _ = require('lodash'),
    util = require('util');

var LOG_DIR = "./Output";
var Monikers = new Array();
var MonikerDBs = new Array();
var parser = new xml2js.Parser();



var daisy_dirs = fs.readdirSync(LOG_DIR);
for ( var dirIndex = 0; dirIndex < daisy_dirs.length; dirIndex++) {
    var daisy_dir = daisy_dirs[dirIndex];

    var files = fs.readdirSync(LOG_DIR + "/" + daisy_dir);
    for ( var fileIndex = 0; fileIndex < files.length; fileIndex++) {
        if (files[fileIndex].match(/^Monikers.xml$/)) {
            MonikerDBs.push({
                'daisyInput' : daisy_dir,
                'filePath' : LOG_DIR + "/" + daisy_dir + "/" + files[fileIndex]
            });
        }
    }
}

var remaining = MonikerDBs.length;

MonikerDBs.forEach(function(parseItem) {

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
                handleParsedMonikers();
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
                    parseMonikerDB(localParseItem, result);

                    //Parse Error or not, decrement remaining
                    //As this is the "deepest aschynchronus callback
                    remaining -= 1;

                    if (remaining == 0) {
                        handleParsedMonikers();
                    }

                }
            });
            //Do work here
        }


    }, { parseItem : parseItem });

    fs.readFile(parseItem.filePath, file_callback);
});

function parseMonikerDB(parseItem, result) {

    for(var monIndex = 0; monIndex < result.MonikerMap.Moniker.length; monIndex++) {
        var moniker = result.MonikerMap.Moniker[monIndex];

        Monikers.push({
            sheet: parseItem.daisyInput,
            name : moniker.Name[0],
            guid : moniker.ContentID[0],
            ct : moniker.ContentType[0]
        })
    }
}


function handleParsedMonikers() {
    //Loop Through Monikers
    console.log("Sheet,Name,ID,Type");
    for(var monIndex = 0; monIndex < Monikers.length; monIndex++) {
        var moniker = Monikers[monIndex];
        console.log(moniker.sheet + "," + moniker.name + "," + moniker.guid + "," + moniker.ct);
    }
}



