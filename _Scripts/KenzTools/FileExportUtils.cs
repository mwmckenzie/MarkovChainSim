// Markov Chain Sim -- FileExportUtils.cs
// 
// Copyright (C) 2022 Matthew W. McKenzie and Kenz LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;
using UnityEngine;

namespace KenzTools
{
    public static class FileExportUtils
    {
        public static void ExportJson(object exportObj, string fileBaseName = "UnityExport",
            string fileIterName = "Data", string dateformat = "yyyy-MM-dd-HHmm-ss")
        {
            var path = Path.GetFullPath("data");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var json = JsonUtility.ToJson(exportObj, true);
            var filename = $"{fileBaseName}_{fileIterName}_{DateTime.Now.ToString(dateformat)}.json";
            File.WriteAllText($"data/{filename}", json);
        }
    }
}