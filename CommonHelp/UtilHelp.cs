using System;
using System.Drawing;
using System.Text.RegularExpressions;
using CMMAuto.Config;
using log4net;

namespace CMMAuto.CommonHelp
{
    public class UtilHelp
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UtilHelp));

        public static bool bytesEqual(byte[] bytes1, byte[] bytes2)
        {
            if (bytes1.Length != bytes2.Length)
            {
                return false;
            }
            for (int i = 0; i < bytes1.Length; i++)
            {
                if (bytes1[i] != bytes2[2])
                {
                    return false;
                }
            }
            return true;
        }

        public static Rectangle GetRectangle(string type)
        {
            var prop = AppSettings.ReadSysValue(type);
            var props = prop.Split(',');
            if (props.Length >= 4)
            {
                int.TryParse(props[0], out int x);
                int.TryParse(props[1], out int y);
                int.TryParse(props[2], out int width);
                int.TryParse(props[3], out int height);
                //log.Info($"[Rect] - Get Rect X: {x}, Y: {y}, Width: {width}, Height: {height}");
                return new Rectangle(x, y, width, height);
            }
            else
            {
                log.Error($"[Rect] - get rect error, cannot access the App.setting for { type }");
                throw new Exception("请先进行标定后再存图...");
            }
        }

        public static string FilterStr(string input)
        {
            //return Regex.Replace(input, "[^a-zA-Z0-9-_\\s]", "");
            var tag = Regex.Replace(input.Trim(), @"[\\/:*?<> |]", "");
            tag = Regex.Replace(tag, @"[_—]", "-");
            if (tag.Length > 0)
            {
                if (tag[tag.Length - 1] == 'V' || tag[tag.Length - 1] == 'v' || tag[tag.Length - 1] == '√')
                {
                    tag = tag.Substring(0, tag.Length - 1);
                }
            }
            return tag.Trim();
        }

        public static Bitmap CropImage(Bitmap sourceBitmap, Rectangle cropRectangle)
        {
            Bitmap croppedBitmap = new Bitmap(cropRectangle.Width, cropRectangle.Height);

            using (Graphics graphics = Graphics.FromImage(croppedBitmap))
            {
                graphics.DrawImage(sourceBitmap, new Rectangle(0, 0, croppedBitmap.Width, croppedBitmap.Height), cropRectangle, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        //public static ModelSettingEntity ConfigJsonToEntity(ModelSettingJson json, string bitmapName, string modelPath = default, Bitmap bitmap = default)
        //{
        //    if (json == null)
        //    {
        //        return null;
        //    }
        //    if (json.Images.Count < 1)
        //    {
        //        //throw new Exception($"[JsonToEntity] 未找到图片相关配置, 参数错误: {json}");
        //        return null;
        //    }
        //    ModelSettingJson.Image image = default;
        //    foreach (var temp in json.Images)
        //    {
        //        if (temp.BitmapName == bitmapName)
        //        {
        //            image = temp;
        //            break;
        //        }
        //    }
        //    return new ModelSettingEntity(json, image, modelPath, bitmap);
        //}

        //public static ModelSettingJson ConfigEntityToJson(List<ModelSettingEntity> entities)
        //{
        //    return new ModelSettingJson(entities);
        //}
    }
}
