using KENDLL.Common;

namespace CMMAuto.Common
{
    class CommonConstant
    {

        public readonly static string TESS_DATA_PATH = @"Resources\Tessdata";
        public readonly static string TESS_DATA_EN = @"eng";
        public readonly static string TESS_DATA_ZH = @"chi_sim_vert";
        public readonly static string IMAGE_PATH = @"images";
        public readonly static string TITLE_CSV_PATH = @"Resources\Title\title.csv";
        public readonly static string YES = @"YES";
        public readonly static string NO = @"NO";
        public readonly static string CONFIG = @"Config";
        public readonly static string CURSOR = @"Cursor";
        public readonly static string TAG = @"Tag";
        public readonly static string MODEL = @"model";
        public readonly static string MODEL_SUFFIX = @"vpp";
        public readonly static string CONFIG_SUFFIX = @"json";
        public readonly static string IMAGE_SUFFIX = @"jpg";
        public readonly static string IMAGE_SUFFIX_JPG = @".jpg";
        public readonly static string SPLIT_FOLDER = @"\";
        public readonly static string CONFIG_PATH = @"config.json";
        public readonly static string BFB = @"%";
        public readonly static string ImageSets = @"ImageSets";
        public readonly static string Image = @"Image";
        public readonly static string Annotation = @"Annotation";
        public readonly static string trainval = @"trainval.txt";
        public readonly static string train = @"train.txt";
        public readonly static string val = @"val.txt";
        public readonly static string INI_CONFIG_FOLDER = "Config";
        public readonly static string INI_CONFIG_FILE_PATH = INI_CONFIG_FOLDER+"/config.ini";
        public static string BASE_URL
        {
            get
            {
                string url = IniFileHandle.getIniKeyValueForStr("Config", "url", INI_CONFIG_FILE_PATH);
                return url;
            }
        }
        public readonly static string URL_DATA = BASE_URL + "/api/data";
        public readonly static string URL_TRAIN = BASE_URL + "/api/train";
        public readonly static string IP_SPLIT = "\\\\";
        public readonly static string F_SPLIT = "\\";
        public readonly static string TrainDataList = "TrainDataList";


        public readonly static string APP_TITLE_KEYWORD = @"TitleKeyword";
        public readonly static string APP_KEYWORD = @"Keyword";
        public readonly static string APP_INSPEC_PATH = @"InspecPath";
        public readonly static string APP_INSPEC_PARAMS = @"InspecParams";
        public readonly static string APP_IMAGE_PATH = @"ImagePath";
        public readonly static string APP_OPEN_IMAGE_PATH = @"OpenImagePath";
        public readonly static string APP_SCREENSHOT_PATH = @"ScreenshotPath";
        public readonly static string APP_MENU = @"Menu";
        public readonly static string APP_IMAGE = @"Image";
        public readonly static string APP_SCREENSHOT = @"Screenshot";
        public readonly static string APP_TAG = @"Tag";
        public readonly static string APP_LOG_EDITOR = @"LogEditor";
        public readonly static string APP_ALGORITHM = @"ALGORITHM";
        public readonly static string APP_COEFFICIENT = @"COEFFICIENT";

    }
}
