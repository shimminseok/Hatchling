using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataTableSt
{
    public struct stItemData
    {
        public string _name;
        public int _type;
        public int _item_kind;
        public int _stat;
        public float _value;
        public string _toolTip;
        public int _price;
        public stItemData(string name, int type, int kind, int stat, float val,string tip,int price)
        {
            _name = name;
            _type = type;
            _item_kind = kind;
            _stat = stat;
            _value = val;
            _toolTip = tip;
            _price = price;
        }
    }
    public struct stCharacterInitData
    {
        public int _level;
        public int _hp;
        public int _mp;
        public int _dam;
        public int _def;
        public int _ex;
        public stCharacterInitData(int lv, int hp, int mp, int dam, int def, int ex)
        {
            _level = lv;
            _hp = hp;
            _mp = mp;
            _dam = dam;
            _def = def;
            _ex = ex;
        }
    }
    public struct stMonsterInitData
    {
        public string _name;
        public int _lv;
        public int _hp;
        public int _ex;
        public int _dam;
        public int _def;
        public float _attDis;
        public float _speed;
        public int _minGold;
        public int _maxGold;
        public stMonsterInitData(string name, int lv, int hp, int ex, int dam, int def, float dis, float sp, int min, int max)
        {
            _name = name;
            _lv = lv;
            _hp = hp;
            _ex = ex;
            _dam = dam;
            _def = def;
            _attDis = dis;
            _speed = sp;
            _minGold = min;
            _maxGold = max;
        }
    }
    public struct stRaidMonsterData
    {
        public string _name;
        public int _hp;
        public float _damage;
        public int _def;
        public float[] _patternDam;
        public int _attDis;
        public int _moveSpeed;
        public stRaidMonsterData(string name, int hp,float damage, int def,float[] patternDam,int dis, int speed)
        {
            _name = name;
            _hp = hp;
            _damage = damage;
            _def = def;
            _patternDam = patternDam;
            _attDis = dis;
            _moveSpeed = speed;
        }
    }
}
