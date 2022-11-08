using System;
using System.IO;
using UnityEngine;

namespace KenzTools {
    public static class FileExportUtils {
        
        public static void ExportJson(object exportObj, string fileBaseName = "UnityExport", 
            string fileIterName = "Data", string dateformat = "yyyy-MM-dd-HHmm-ss") {
            var path = Path.GetFullPath("data");
            if (!Directory.Exists(path)) { Directory.CreateDirectory(path); }
            
            var json = JsonUtility.ToJson(exportObj, true);
            var filename = $"{fileBaseName}_{fileIterName}_{DateTime.Now.ToString(dateformat)}.json";
            File.WriteAllText($"data/{filename}", json);
            //File.WriteAllText($"D:/Unity/Projects/CardGameScratchSpace/Assets/data/{filename}", json);
        }
    }
}