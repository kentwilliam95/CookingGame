using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cooking.Database
{
    public class UserData
    {

        private static List<LevelGroup> levelGroups;
        public static List<LevelGroup> LevelGroups => levelGroups;
        public static int Coins { get; private set; }
        public static int SelectedGroup { get; private set; }
        public static void Initialize()
        {
            InitializeFirstDatabase();
        }

        public static void SelectGroup(int group)
        {
            SelectedGroup = group;
        }

        public static void InitializeFirstDatabase()
        {
            levelGroups = new List<LevelGroup>(2);
            for (int i = 0; i < 2; i++)
            {
                LevelGroup levelGroup = new LevelGroup(i + 1);

                for (int j = 0; j < 3; j++)
                {
                    Level lvl = null;
                    if (j == 0)
                        lvl = new LevelNormal();
                    else
                        lvl = new levelDebug();

                    lvl.Initialize(i + 1, j + 1);
                    levelGroup.AddLevel(lvl);
                }
                levelGroups.Add(levelGroup);
            }

            levelGroups[1].levelRequirement.Add(levelGroups[0].levels[0]);
            ////Test
            //Debug.Log(CompleteLevel(0, 1));
            //Debug.Log(CompleteLevel(0, 2));
            //Debug.Log(CompleteLevel(0, 3));
            //Debug.Log(CompleteLevel(0, 4));
            //Debug.Log(CompleteLevel(0, 5));

            //Debug.Log(CompleteLevel(1, 1));
            //Debug.Log(CompleteLevel(1, 2));
            //Debug.Log(CompleteLevel(1, 3));
            //Debug.Log(CompleteLevel(1, 4));
            //Debug.Log(CompleteLevel(1, 5));

            //Debug.Log(CompleteLevel(2, 1));
            //Debug.Log(CompleteLevel(2, 2));
            //Debug.Log(CompleteLevel(2, 3));
            //Debug.Log(CompleteLevel(2, 4));
            //Debug.Log(CompleteLevel(2, 5));

            //Debug.Log(CompleteLevel(-2, -1));
            //Debug.Log(CompleteLevel(2, -2));
            //Debug.Log(CompleteLevel(-2, 3));
            //Debug.Log(CompleteLevel(2, -4));
            //Debug.Log(CompleteLevel(-2, 5));
        }

        public static List<LevelGroup> GetLevelGroup()
        {
            return levelGroups;
        }

        //public static bool CompleteLevel(int group, int level = 1)
        //{
        //    int maxGroup = levelGroups.Count;
        //    if (group > maxGroup - 1 || group < 0)
        //        return false;

        //    var levelGroup = levelGroups[group];
        //    level = level - 1;

        //    if (level > levelGroup.MaxLevel - 1 || level < 0)
        //        return false;

        //    levelGroup.CompleteLevel(level);
        //    return true;
        //}

        public static bool CompleteLevel(int group)
        {
            group = group - 1;
            int maxGroup = levelGroups.Count;
            if (group > maxGroup - 1 || group < 0)
                return false;

            var levelGroup = levelGroups[group];
            if (levelGroup.IsGroupComplete())
                return false;

            levelGroup.CompleteLevel();
            return true;
        }

        public static void AddCoins(int amount)
        {
            Coins = Mathf.Max(Coins + amount, 0);
        }
    }

    public class LevelGroup
    {
        private bool isUnlock;

        public List<Level> levels = new List<Level>();
        public List<Level> levelRequirement = new List<Level>();

        public int CurrentLevel => CurrentLevelIndex + 1;
        public int CurrentLevelIndex { get; private set; }
        public int MaxLevel { get; private set; }
        public int GroupID { get; }
        public int GroupIndex => GroupID - 1;
        public string LevelName => $"{GroupID}-{CurrentLevel}";

        public Action OnLevelComplete;

        public LevelGroup(int group)
        {
            this.GroupID = group;
        }

        public bool IsGroupLevelValid()
        {
            if (isUnlock)
                return isUnlock;

            foreach (var item in levelRequirement)
            {
                if (!item.isComplete)
                    return false;
            }
            isUnlock = true;
            return true;
        }

        public bool IsGroupComplete()
        {
            foreach (var item in levels)
            {
                if (!item.isComplete)
                    return false;
            }
            return true;
        }

        public bool IsLevelComplete(int level)
        {
            level = level - 1;
            if (level < 0 || level > levels.Count)
                return false;

            return levels[level].isComplete;
        }

        public void AddLevel(Level lvl)
        {
            levels.Add(lvl);
            MaxLevel = levels.Count;
        }

        public void AddLevelRequirement(Level lvl)
        {
            levelRequirement.Add(lvl);
        }

        public void CompleteLevel(int lvl)
        {
            levels[lvl].isComplete = true;
            new EventLevelComplete().Invoke();
        }

        public void CompleteLevel()
        {
            //Debug.Log($"{GroupID}-{CurrentLevelIndex} Complete!");
            levels[CurrentLevelIndex].isComplete = true;
            CurrentLevelIndex = Mathf.Clamp(CurrentLevelIndex + 1, 0, MaxLevel - 1);
            new EventLevelComplete().Invoke();
        }

        public Level GetLevel()
        {
            return levels[CurrentLevelIndex];
        }
    }

    public class Level
    {
        protected int group;
        protected int level;

        public int Group => group;

        public bool isComplete;
        public void Initialize(int group, int level)
        {
            this.group = group;
            this.level = level;
        }

        public virtual void LoadLevel() { }
    }

    public class LevelNormal : Level
    {
        public override void LoadLevel()
        {
            Core.AppController.Instance.LoadGameScene(false);
        }
    }

    public class levelDebug : Level
    {
        public override void LoadLevel()
        {
            UserData.CompleteLevel(group);
            Debug.Log($"Level {group}-{level} Completed!");
        }
    }
}