using DefineServerUtility;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public struct stUserInfo
{
    public long _uuid;
    public string _id;
    public string _pw;
    public string _nickName;
    public int _level;
    public int _hp;
    public int _mp;
    public int _curEx;
    public int _curMoney;
    public float[] _pos;
    public byte[] _inventory;
    public byte[] _mountItem;
}
class DBController
{
    string _dbName;
    public static MySqlConnection _connection;
    public const string _baselocalIP = "localhost";
    public const int _port = 3306;
    public static string _tableName = "";
    public static List<string> _culumns = new List<string>();


    public string _value = "Value ('{0}','{1}','{2}',{3},{4},{5},{6});";

    public static stUserInfo _userInfo = new stUserInfo();
    public List<byte> _userItemInfo = new List<byte>();
    public DBController(string name)
    {
        _dbName = name;
        ConnectDB("root", "");
    }
    public bool ConnectDB(string id, string pw)
    {
        string connectionInfoText = string.Format("Server={0};Port={1};DataBase={2};Uid={3};Pwd={4}", _baselocalIP, _port, _dbName, id, pw);
        try
        {
            _connection = new MySqlConnection(connectionInfoText);
            _connection.Open();
            Console.WriteLine("DB접속에 성공 했습니다.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("DB접속에 실패 했습니다.");
            Console.WriteLine(ex.ToString());
            return false;
        }
    }
    public bool Insert(string id, string pw)
    {
        try
        {
            string val = string.Format("Value ({0}, '{1}', '{2}', '');", 0, id, pw);
            string sql = string.Format("INSERT INTO {0} {1}", "hachling_userinfo.signupinfo", val);
            MySqlCommand cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return false;
    }
    public void Insert(string nick)
    {
        try
        {
            string val = string.Format("Value ('{0}');", nick);
            string sql = string.Format("INSERT INTO {0} (NickName) {1}", "hachling_userinfo.userinfo", val);
            MySqlCommand cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            val = string.Format("Value ('{0}');", nick);
            sql = string.Format("INSERT INTO {0} (NickName) {1}", "hachling_userinfo.inventory", val);
            cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
            val = string.Format("Value ('{0}');", nick);
            sql = string.Format("INSERT INTO {0} (NickName) {1}", "hachling_userinfo.mountitem", val);
            cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public bool GetUserInfo(eReceiveMessage cheak, stUserInfo userinfo, string tableName, out stUserInfo _userinfo)
    {
        string sql = string.Empty;
        switch (cheak)
        {
            case eReceiveMessage.DuplicateID:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, "ID", userinfo._id);
                break;
            case eReceiveMessage.UserLoginInfo:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}' AND {3} = {4} ORDER BY UUID", tableName, "ID", userinfo._id, "PW", userinfo._pw);
                break;
            case eReceiveMessage.DuplicateNick:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, "NickName", userinfo._nickName);
                break;
            case eReceiveMessage.UserInfo:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, "NickName", userinfo._nickName);
                break;
            case eReceiveMessage.UpdateItemInfo:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, "NickName", userinfo._nickName);
                break;
            case eReceiveMessage.UpdateMountItem:
                sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", tableName, "NickName", userinfo._nickName);
                break;
        }
        return TableReader(cheak, sql, userinfo, out _userinfo);
    }
    bool TableReader(eReceiveMessage cheak, string sql, stUserInfo userinfo, out stUserInfo _userinfo)
    {
        MySqlCommand cmd = new MySqlCommand(sql, _connection);
        MySqlDataReader table = cmd.ExecuteReader();
        switch (cheak)
        {
            case eReceiveMessage.DuplicateID:
                while (table.Read())
                {
                    if (userinfo._id.Equals(string.Format("{0}", table["ID"])))
                    {
                        _userinfo = userinfo;
                        table.Close();
                        cmd.ExecuteNonQuery();
                        return false;
                    }
                }
                break;
            case eReceiveMessage.UserLoginInfo:
                while (table.Read())
                {
                    if (userinfo._id.Equals(string.Format("{0}", table["ID"])) && userinfo._pw.Equals(string.Format("{0}", table["PW"])))
                    {
                        //아이디가 존재
                        userinfo._nickName = string.Format("{0}", table["NickName"]);
                        _userinfo = userinfo;
                        table.Close();
                        cmd.ExecuteNonQuery();
                        return false;
                    }
                }
                break;
            case eReceiveMessage.DuplicateNick:
                while (table.Read())
                {
                    if (userinfo._nickName.Equals(string.Format("{0}", table["NickName"])))
                    {
                        //닉네임 존재
                        _userinfo = userinfo;
                        table.Close();
                        cmd.ExecuteNonQuery();
                        return false;
                    }
                }
                break;
            case eReceiveMessage.UserInfo:
                while (table.Read())
                {
                    if (userinfo._nickName.Equals(string.Format("{0}", table["NickName"])))
                    {
                        userinfo._pos = new float[3];
                        //닉네임 존재
                        userinfo._level = int.Parse(string.Format("{0}", table["Level"]));
                        userinfo._hp = int.Parse(string.Format("{0}", table["HP"]));
                        userinfo._mp = int.Parse(string.Format("{0}", table["MP"]));
                        userinfo._curMoney = int.Parse(string.Format("{0}", table["Money"]));
                        userinfo._curEx = int.Parse(string.Format("{0}", table["CurEx"]));
                        userinfo._pos[0] = float.Parse(string.Format("{0}", table["Position_X"]));
                        userinfo._pos[1] = float.Parse(string.Format("{0}", table["Position_Y"]));
                        userinfo._pos[2] = float.Parse(string.Format("{0}", table["Position_Z"]));
                        _userinfo = userinfo;
                        table.Close();
                        cmd.ExecuteNonQuery();
                        return false;
                    }
                }
                break;
            case eReceiveMessage.UpdateItemInfo:
                _userItemInfo.Clear();
                while (table.Read())
                {
                    if (userinfo._nickName.Equals(string.Format("{0}", table["NickName"])))
                    {
                        for (int n = 0; n < table.FieldCount / 3; n++)
                        {
                            _userItemInfo.Add(byte.Parse(string.Format("{0}", table["Index_" + n])));
                            _userItemInfo.Add(byte.Parse(string.Format("{0}", table["Item_" + n])));
                            _userItemInfo.Add(byte.Parse(string.Format("{0}", table["Amount_" + n])));
                        }
                        userinfo._inventory = _userItemInfo.ToArray();
                    }
                }
                break;
            case eReceiveMessage.UpdateMountItem:
                List<byte> mountItem = new List<byte>();
                while (table.Read())
                {
                    if (userinfo._nickName.Equals(string.Format("{0}", table["NickName"])))
                    {
                        for (int n = 1; n < table.FieldCount; n++)
                        {
                            mountItem.Add(byte.Parse(string.Format("{0}", table[n])));
                        }
                    }
                }
                userinfo._mountItem = mountItem.ToArray();
                break;
        }
        table.Close();
        cmd.ExecuteNonQuery();
        _userinfo = userinfo;
        return true;

    }
    public void UpdateData(eReceiveMessage type, string tableName, stUserInfo userinfo, List<byte> item = null)
    {
        string sql = string.Empty;
        try
        {
            MySqlCommand cmd;
            switch (type)
            {
                case eReceiveMessage.InsertNickName:
                    sql = string.Format("UPDATE {0} SET {1} = '{2}'  WHERE {3} = '{4}';", tableName, "NickName", userinfo._nickName, "ID", userinfo._id);
                    break;
                case eReceiveMessage.UpdateUserInfo:
                    switch (tableName)
                    {
                        case "hachling_userinfo.userinfo":
                            sql = string.Format("UPDATE {0} SET {1} = {2}, {3} = {4}, {5} = {6}, {7} = {8}, {9} = {10}, {11} = {12}, {13} = {14}, {15} = {16} WHERE {17} = '{18}';", tableName,
                                "Level", userinfo._level, "HP", userinfo._hp, "MP", userinfo._mp, "Money", userinfo._curMoney, "CurEx", userinfo._curEx, "Position_X", userinfo._pos[0], "Position_Y", userinfo._pos[1], "Position_Z", userinfo._pos[2], "NickName", userinfo._nickName);
                            break;
                        case "hachling_userinfo.inventory":
                            for (int n = 0; n < (userinfo._inventory.Length) / 3; n++)
                            {
                                for (int m = n; m < n + 1; m++)
                                {
                                    sql = string.Format("UPDATE {0} SET {1} = {2}, {3} = {4}, {5} = {6} WHERE {7} = '{8}';", tableName, "Index_" + n, userinfo._inventory[(m * 2) + n], "Item_" + n, userinfo._inventory[(m * 2) + (n + 1)], "Amount_" + n, userinfo._inventory[(m * 2) + (n + 2)], "NickName", userinfo._nickName);
                                    cmd = new MySqlCommand(sql, _connection);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            return;
                        case "hachling_userinfo.mountitem":

                            sql = string.Format("UPDATE {0} SET {1} = {2}, {3} = {4}, {5} = {6},{7} = {8},{9} = {10} WHERE {11} = '{12}';", tableName, "Helmat_Kind", userinfo._mountItem[0], "Armor_Kind", userinfo._mountItem[1],
                                "Weapon_Kind", userinfo._mountItem[2], "Ring_Kind", userinfo._mountItem[3], "Shouse_Kind", userinfo._mountItem[4], "NickName", userinfo._nickName);
                            break;
                    }
                    break;
                case eReceiveMessage.UpdateItemInfo:
                    switch (tableName)
                    {
                        case "hachling_userinfo.userinfo":
                            for (int n = 0; n < (userinfo._inventory.Length) / 3; n++)
                            {
                                for (int m = n; m < n + 1; m++)
                                {
                                    sql = string.Format("UPDATE {0} SET {1} = {2}, {3} = {4}, {5} = {6} WHERE {7} = '{8}';", tableName, "Index_" + n, userinfo._inventory[(m * 2) + n], "Item_" + n, userinfo._inventory[(m * 2) + (n + 1)], "Amount_" + n, userinfo._inventory[(m * 2) + (n + 2)], "NickName", userinfo._nickName);
                                    cmd = new MySqlCommand(sql, _connection);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            break;
                        case "hachling_userinfo.inventory":
                            break;
                        case "hachling_userinfo.mountitem":
                            break;
                    }
                    return;
            }
            cmd = new MySqlCommand(sql, _connection);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

