using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Inference
{

    [CreateAssetMenu(menuName = "ClueData")]
    public class ClueDataStroage : ScriptableObject
    {
        public TextAsset data;
        [Header("시작 줄")]
        public int START_LINE;
        public Clue[] clues;
        public Clue[] sample;

        public ClueRecipe[] recipes;

        [Header("빌드 전용")]
        public bool isForBuild = false;

        public Dictionary<string, IClue> clueCollection = new();

        [ContextMenu("파일에서 데이터 불러오기")]
        public void Convert()
        {
            // -----------------------------
            // 1. 줄 이어붙이기 (description 안에 \n이 들어간 경우)
            // -----------------------------
            List<string> mergedLines = new List<string>();
            bool insideQuotes = false;
            string currentLine = "";

            byte[] bytes = data.bytes;

            var text = Encoding.UTF8.GetString(
                Encoding.Convert(Encoding.GetEncoding("euc-kr"), Encoding.UTF8, bytes)
                );

            foreach (var rawLine in text.Split('\n'))
            {
                var line = rawLine.TrimEnd('\r');

                // description 시작 전
                if (!insideQuotes)
                    currentLine = line;
                else
                    currentLine += "\n" + line; // description 이어붙이기

                int quoteCount = line.Count(c => c == '"');
                if (quoteCount % 2 != 0) insideQuotes = !insideQuotes;

                // description이 닫힌 시점에서 최종적으로 한 줄로 합쳐짐
                if (!insideQuotes)
                {
                    mergedLines.Add(currentLine);
                    currentLine = "";
                }
            }

            // -----------------------------
            // 2. 병합된 라인 기준으로 파싱 시작
            // -----------------------------
            List<Clue> clues = new List<Clue>();
            int lineIndex = 0;

            foreach (var rawline in mergedLines)
            {
                lineIndex++;
                if (lineIndex <= START_LINE) continue;

                if (string.IsNullOrWhiteSpace(rawline)) continue;

                Clue clue = new Clue();
                var line = rawline.Split(',');

                for (int i = 0; i < line.Length; i++)
                {
                    string field = line[i].Trim().Trim('"');

                    switch (i)
                    {
                        case 0:
                            clue.Id = field;
                            break;
                        case 1:
                            clue.imagePath = field;
                            break;
                        case 2:
                            clue.Name = field;
                            break;
                        case 3:
                            clue.Tags = field.Split('/');
                            break;
                        case 4:
                            // description 필드에서 \n 제거 후 / 로 split
                            string[] descs = field.Split(new char[] { '/' }, System.StringSplitOptions.RemoveEmptyEntries);

                            ClueDescription[] clueDescs = new ClueDescription[descs.Length];
                            for (int j = 0; j < descs.Length; j++)
                            {
                                string desc = descs[j].Trim();
                                clueDescs[j] = new ClueDescription(desc, j);
                            }
                            clue.Description = clueDescs;
                            break;
                    }
                }

                clues.Add(clue);
            }

            sample = clues.ToArray();

            Debug.Log($"{sample.Length}개의 단서 데이터를 불러왔습니다.");
        }

        public IClue GetClue(string id)
        {
            for (int i = 0; i < clueCollection.Count; i++)
            {
                if (clueCollection.TryGetValue(id, out IClue clue))
                    return clue;
            }

            Debug.LogError($"{id} clue is not exist. Check the Clue Data");
            return null;
        }


        [ContextMenu("Json으로 변환")]
        public void ToJsonFile()
        {

        }
    }
}
