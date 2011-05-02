using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Munger
{
    class LinkSubstituter
    {
        static Dictionary<string, string> _replacementMap = new Dictionary<string, string>();

        static LinkSubstituter()
        {
            _replacementMap.Add("/recoveryobjectives", "/aboutnci/recovery/recoveryobjectives");
            _replacementMap.Add("/espanol/instituto/olacpd/page6", "/espanol/instituto/olacpd/acerca");
            _replacementMap.Add("/recoveryimpact/fundedresearchers/page1", "/aboutnci/recovery/communityimpact/fundedresearchers");
            _replacementMap.Add("/director", "/aboutnci/director");
            _replacementMap.Add("/aboutnci/director-announced/varmus", "/aboutnci/director");
            _replacementMap.Add("/aboutnci/servingpeople/advances", "/aboutnci/servingpeople/cancer-research-progress/advances");
            _replacementMap.Add("/aboutnci/servingpeople/burden", "/aboutnci/servingpeople/cancer-statistics");
            _replacementMap.Add("/aboutnci/servingpeople/cancer-research-progress/micrornaliver", "/aboutnci/servingpeople/cancer-research-progress/advances/micrornaliver");
            _replacementMap.Add("/aboutnci/servingpeople/costofcancer", "/aboutnci/servingpeople/cancer-statistics/costofcancer");
            _replacementMap.Add("/aboutnci/servingpeople/disease_snapshots", "/aboutnci/servingpeople/cancer-statistics/snapshots");
            _replacementMap.Add("/aboutnci/servingpeople/paclitaxel", "/aboutnci/servingpeople/cancer-research-progress/advances/paclitaxel");
            _replacementMap.Add("/aboutnci/servingpeople/roi", "/aboutnci/servingpeople/nci-budget-information/spending");
            _replacementMap.Add("/aboutnci/servingpeople/snapshot", "/aboutnci/servingpeople/nci-budget-information/fy2009");
            _replacementMap.Add("/benchmarks/archives/2003_04", "http://benchmarks.cancer.gov/2003/");
            _replacementMap.Add("/cancer_information/cancer_type/breast", "/cancertopics/types/breast");
            _replacementMap.Add("/cancerinfo/pdq/treatment/cml/patient", "/cancertopics/pdq/treatment/cml/patient");
            _replacementMap.Add("/cancertopics/factsheet/brain-tumors-cell-phones", "/newscenter/brain-tumors-cell-phonesQandA");
            _replacementMap.Add("/cancertopics/factsheet/nci/cancer-centers", "https://cissecure.nci.nih.gov/factsheet/FactSheetSearch1_2.aspx");
            _replacementMap.Add("/cancertopics/factsheet/prevention/select", "/newscenter/pressreleases/selectqanda");
            _replacementMap.Add("/cancertopics/factsheet/starresultsqanda", "/newscenter/starresultsqanda");
            _replacementMap.Add("/clinical_trials", "/clinicaltrials");
            _replacementMap.Add("/clinicaltrials/basic-search-form-help", "/clinicaltrials/search");
            _replacementMap.Add("/clinicaltrials/crad001c2240", "/search/viewclinicaltrials.aspx?version=healthprofessional&cdrid=527261");
            _replacementMap.Add("/espanol/cancer/hojas-informativas/estudio-clinico-star-resultados-respuestas", "/espanol/noticias/estudio-clinico-star-resultados-respuestas");
            _replacementMap.Add("/hurricane-response-efforts", "/aboutnci/emergency/hurricanes");
            _replacementMap.Add("/hurricane-response-efforts/espanol", "/espanol/instituto/emergencia/huracan");
            _replacementMap.Add("/katrina", "/aboutnci/emergency/hurricanes");
            _replacementMap.Add("/i131/fallout", "/cancertopics/causes/i131");
            _replacementMap.Add("/i131/fallout/html", "/cancertopics/causes/i131");
            _replacementMap.Add("/licensing", "/global/syndication/content-use");
            _replacementMap.Add("/newscenter/benchmarks-vol1-issue3", "http://benchmarks.cancer.gov/2001/09/digital-mammography-in-the-21st-century/");
            _replacementMap.Add("/newscenter/benchmarks-vol2-issue1", "http://benchmarks.cancer.gov/2002/11/cancer-prevention-on-the-move/");
            _replacementMap.Add("/newscenter/benchmarks-vol2-issue2", "http://benchmarks.cancer.gov/2002/02/proteomics-research-for-the-21st-century/");
            _replacementMap.Add("/newscenter/benchmarks-vol2-issue4", "	http://benchmarks.cancer.gov/2002/04/bethesda-2001-a-revised-system-for-reporting-pap-test-results/");
            _replacementMap.Add("/newscenter/benchmarks-vol2-issue5", "	http://benchmarks.cancer.gov/2002/05/2002-report-to-the-nation/");
            _replacementMap.Add("/newscenter/benchmarks-vol2-issue9", "	http://benchmarks.cancer.gov/2002/09/searching-for-a-lung-cancer-screening-test/");
            _replacementMap.Add("/newscenter/benchmarks-vol6-issue1/page2", "http://benchmarks.cancer.gov/2006/02/turning-molecules-into-medicine-role-of-nci/");
            _replacementMap.Add("/newscenter/benchmarks-vol8-issue1", "http://benchmarks.cancer.gov/2009/03/robotic-prostatectomy/");
            _replacementMap.Add("/newscenter/pressreleases/assistqanda", "/newscenter/qa/2004/assistqanda");
            _replacementMap.Add("/newscenter/pressreleases/b-roll-treatment", "http://benchmarks.cancer.gov/nci-b-roll-collection/");
            _replacementMap.Add("/newscenter/pressreleases/dmistqanda", "/newscenter/qa/2005/dmistqanda");
            _replacementMap.Add("/newscenter/pressreleases/letrozoleqanda", "/newscenter/qa/2003/letrozoletamoxifenqanda");
            _replacementMap.Add("/newscenter/pressreleases/starresultsqanda", "/newscenter/qa/2006/starresultsqanda");
            _replacementMap.Add("/research_funding/grants", "/researchandfunding/grantprocess");
            _replacementMap.Add("/researchfunding", "/researchandfunding");
            _replacementMap.Add("/search/geneticsservices", "/cancertopics/genetics/directory");
            _replacementMap.Add("/search/clinicaltrials", "/clinicaltrials/search");
            _replacementMap.Add("/hints", "http://hints.cancer.gov/");
        }

        /// <summary>
        /// Replaces a URL with one from the list of substitutes.
        /// If no replacement exists, the original URL is returned instead.
        /// </summary>
        /// <param name="originalUrl">URL to replace</param>
        /// <returns>A replacement URL.</returns>
        public string MakeSubstitution(string originalUrl)
        {
            string replacement = originalUrl;

            // All keys are lowercase.
            string key = originalUrl.ToLower();

            if (_replacementMap.ContainsKey(key))
            {
                replacement = _replacementMap[key];
            }
            else if (originalUrl.StartsWith("/directorscorner/")
                || originalUrl.StartsWith("/aboutnci/directorscorner"))
            {
                replacement="/aboutnci/director";
            }

            return replacement;
        }
    }
}
