using System;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

[Serializable]
public class nft
{
    public int id;
    public string title;
    public string currency;
    public string owner;
    public float btc;
    public float eth;
    public float ltc;
    public float eur;
}

[Serializable]
public class userData
{
    public int id;
    public string pseudo;
    public float btc;
    public float eth;
    public float ltc;
    public float eur;
}

public class SqliteSetBdd : MonoBehaviour
{
    private static string hash = "123987@!abc";
    private string url;
    private IDbConnection dbcon;
    
    private GetCryptoData gcd;
    
    [SerializeField] public List<nft> nftList = new List<nft>();
    public userData ud;
    
    [Header("Images")]
    public List<Sprite> nftImages;
    public List<Material> nftMaterials;
    
    void Start()
    {
        // --- INITIALIZE COMPONENTS
        gcd = FindObjectOfType<GetCryptoData>();
        url = "URI=file:" + Application.persistentDataPath + "/My_Database";

        // --- READ NFT TABLE (IF EXIST) TO CHECK IF DATABASE EXISTS
        dbcon = InitConnection();
        dbcon.Open();
        DropTables(dbcon);
        ReadNftTable(dbcon);
        dbcon.Close();
        
        // --- GET CRYPTO DATA (& CREATE DATABASE IF NOT EXIST)
        gcd.GetData();
    }

    IDbConnection InitConnection()
    {
        string connection = url;
        IDbConnection dbcon = new SqliteConnection(connection);
        return dbcon;
    }

    public void StartInitDatabase()
    {
        // --- DATABASE INITIALIZER (RUN BY GCD IF DATABASE DOES NOT EXIST)
        
        // --- INITIALIZE CONNECTION
        dbcon = InitConnection();
        dbcon.Open();
        
        // --- INITIALIZE TABLES
        // DropTables(dbcon);
        InitNftTable(dbcon);
        InitUserTable(dbcon);
        
        // --- READ DATABASE
        ReadNftTable(dbcon);
        
        // --- CLOSE CONNECTION
        dbcon.Close();
    }

    void InitNftTable(IDbConnection conn)
    {
        string[] data =
        {
            // "(ID, TITLE , REFERENCE, OWNER, BtcWhenBought, EthWhenBought, LtcWhenBought, EurWhenBought)",
            "(0, 'chat_noir' , 'EUR', '','"+ CTP(EurToBtc(57.62f)) +"','"+ CTP(EurToEth(57.62f)) +"','"+ CTP(EurToLtc(57.62f)) +"',    57.62               )",
            "(1, 'chat_potté', 'EUR', '','"+ CTP(EurToBtc(32.51f)) +"','"+ CTP(EurToBtc(32.51f)) +"','"+ CTP(EurToLtc(32.51f)) +"',    32.51               )",
            "(2, 'piano_cat' , 'BTC', '',    0.002               ,'"+ CTP(BtcToEth(0.002f)) +"','"+ CTP(BtcToLtc(0.002f)) +"','"+ CTP(BtcToEur(0.002f)) +"')",
            "(3, 'nyan_cat'  , 'ETH', '','"+ CTP(EthToBtc(00.03f)) +"',    0.03                ,'"+ CTP(EthToLtc(00.03f)) +"','"+ CTP(EthToEur(00.03f)) +"')",
            "(4, 'alice_cat' , 'ETH', '','"+ CTP(EthToBtc(000.7f)) +"',    0.7                 ,'"+ CTP(EthToLtc(00.03f)) +"','"+ CTP(EthToEur(00.03f)) +"')"
        };
        
        // CREATE NTF TABLE
        IDbCommand dbcmd;
        dbcmd = conn.CreateCommand();
        string q_createTable = 
            "CREATE TABLE IF NOT EXISTS nft_table (id INTEGER PRIMARY KEY, title VARCHAR, reference VARCHAR, owner VARCHAR NULLABLE, btc FLOAT, eth FLOAT, ltc FLOAT, eur FLOAT)";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
        
        // FILL TABLE WITH DATA
        IDbCommand cmnd = conn.CreateCommand();
        foreach (string s in data)
        {
            // Insert or ignore
            cmnd.CommandText = "INSERT OR IGNORE INTO nft_table (id, title, reference, owner, btc, eth, ltc, eur) VALUES " + s;
            cmnd.ExecuteNonQuery();
        }
    }
    
    void InitUserTable(IDbConnection conn)
    {
        // (ID, PSEUDO , PWD, BTC, ETH, LTC, EUR)
        string user0 = "(0, '" + Encrypt("user0") + "', '" + Encrypt("0000") + "', .0026, .0351, .6411, 165.35)";
        string user1 = "(1, '" + Encrypt("user1") + "', '" + Encrypt("1111") + "', .0001, .0025, .0014,  27.19)";
        string user2 = "(2, '" + Encrypt("user2") + "', '" + Encrypt("2222") + "', .0009, .0034, .0249, 119.54)";
        string[] data = { user0, user1, user2 };
        
        // CREATE NTF TABLE
        IDbCommand dbcmd;
        dbcmd = conn.CreateCommand();
        string q_createTable = 
            "CREATE TABLE IF NOT EXISTS user_table (id INTEGER PRIMARY KEY, pseudo VARCHAR, pwd VARCHAR, btc FLOAT, eth FLOAT, ltc FLOAT, eur FLOAT)";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
        
        // FILL TABLE WITH DATA
        IDbCommand cmnd = conn.CreateCommand();
        foreach (string s in data)
        {
            // Insert or ignore
            cmnd.CommandText = "INSERT OR IGNORE INTO user_table (id, pseudo, pwd, btc, eth, ltc, eur) VALUES " + s;
            cmnd.ExecuteNonQuery();
        }
    }
    
    void ReadNftTable(IDbConnection conn)
    {
        // READ NFT TABLE IF EXISTS & FILL NFT PUBLIC LIST
        
        IDbCommand cmnd_read_secure = conn.CreateCommand();
        IDbCommand cmnd_read = conn.CreateCommand();
        IDataReader reader;

        string secureQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='nft_table'";
        cmnd_read_secure.CommandText = secureQuery;
        reader = cmnd_read_secure.ExecuteReader();
        if (reader[0].ToString() != "nft_table") return;
        
        string query = "SELECT * FROM nft_table";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        if (nftList.Count != 0)
            nftList.Clear();
        
        while (reader.Read())
        {
            nft sample = new nft();

            sample.id       = int.Parse(reader[0].ToString());
            sample.title    = reader[1].ToString();
            sample.currency = reader[2].ToString();
            sample.owner    = reader[3].ToString();
            sample.btc      = float.Parse(reader[4].ToString());
            sample.eth      = float.Parse(reader[5].ToString());
            sample.ltc      = float.Parse(reader[6].ToString());
            sample.eur      = float.Parse(reader[7].ToString());
            
            nftList.Add(sample);
        }
    }

    public void BuyNft(int nftId, string currency, float price)
    {
        // --- INITIALIZE CONNECTION
        dbcon = InitConnection();
        dbcon.Open();
        BuyNft(dbcon, nftId, currency, price);
    }
    public void BuyNft(IDbConnection conn, int nftId, string currency, float price)
    {
        IDbCommand cmnd_update = conn.CreateCommand();
        
        // --- UPDATE OWNER
        string query = "UPDATE nft_table SET owner = '" + ud.pseudo + "' WHERE id = '" + nftId + "'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        // --- UPDATE REFERENCE
        query = "UPDATE nft_table SET reference = '" + currency + "' WHERE id = '" + nftId +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        // --- UPDATE REFERENCE VALUE
        if (currency == "BTC")
        {
            query = "UPDATE nft_table SET btc = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "ETH")
        {
            query = "UPDATE nft_table SET eth = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "LTC")
        {
            query = "UPDATE nft_table SET ltc = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else
        {
            query = "UPDATE nft_table SET eur = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        
        // --- UPDATE OTHER CURRENCIES
        UpdateNftValuesByReference(conn, currency, price, nftId);
        
        // --- UPDATE USER WALLET
        if (currency == "BTC")
        {
            query = "UPDATE user_table SET btc = '" + CTP(ud.btc - price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "ETH")
        {
            query = "UPDATE user_table SET eth = '" + CTP(ud.eth - price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "LTC")
        {
            query = "UPDATE user_table SET ltc = '" + CTP(ud.ltc - price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else
        {
            query = "UPDATE user_table SET eur = '" + CTP(ud.eur - price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        
        // --- READ & UPDATE NFT LIST
        ReadNftTable(conn);

        // --- READ & UPDATE USER DATA
        ReadUserData(conn);
        
        // --- DISCONNECTION
        dbcon.Close();
    }
    
    public void SaleNft(int nftId, string currency, float price)
    {
        // --- INITIALIZE CONNECTION
        dbcon = InitConnection();
        dbcon.Open();
        SaleNft(dbcon, nftId, currency, price);
    }
    public void SaleNft(IDbConnection conn, int nftId, string currency, float price)
    {
        IDbCommand cmnd_update = conn.CreateCommand();
        
        // --- UPDATE OWNER
        string query = "UPDATE nft_table SET owner = '' WHERE id = '" + nftId + "'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        // --- UPDATE REFERENCE
        query = "UPDATE nft_table SET reference = '" + currency + "' WHERE id = '" + nftId +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        // --- UPDATE REFERENCE VALUE
        if (currency == "BTC")
        {
            query = "UPDATE nft_table SET btc = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "ETH")
        {
            query = "UPDATE nft_table SET eth = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "LTC")
        {
            query = "UPDATE nft_table SET ltc = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else
        {
            query = "UPDATE nft_table SET eur = '" + CTP(price) + "' WHERE id = '" + nftId +"'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        
        // --- UPDATE OTHER CURRENCIES
        UpdateNftValuesByReference(conn, currency, price, nftId);
        
        // --- UPDATE USER WALLET
        if (currency == "BTC")
        {
            query = "UPDATE user_table SET btc = '" + CTP(ud.btc + price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "ETH")
        {
            query = "UPDATE user_table SET eth = '" + CTP(ud.eth + price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else if (currency == "LTC")
        {
            query = "UPDATE user_table SET ltc = '" + CTP(ud.ltc + price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        else
        {
            query = "UPDATE user_table SET eur = '" + CTP(ud.eur + price) + "' WHERE id = '" + ud.id + "'";
            cmnd_update.CommandText = query;
            cmnd_update.ExecuteNonQuery();
        }
        
        // --- READ & UPDATE NFT LIST
        ReadNftTable(conn);

        // --- READ & UPDATE USER DATA
        ReadUserData(conn);
        
        // --- DISCONNECTION
        dbcon.Close();
    }
    
    public bool GetUserByLogin(string login, string password)
    {
        // SEARCH IN user_table TO VALIDATE LOGIN ACTION
        
        // --- CONNECTION
        dbcon = InitConnection();
        dbcon.Open();
        
        // --- INIT COMMAND
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;

        // --- READ IN TABLE
        string query = "SELECT * FROM user_table WHERE pseudo = '" + Encrypt(login) + "' AND pwd = '" + Encrypt(password) + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        // --- USE BROUGHT DATA (& FILL USER DATA WITH)
        while (reader.Read())
        {
            userData newUd = new userData();

            newUd.id        = int.Parse(reader[0].ToString());
            newUd.pseudo    = Decrypt(reader[1].ToString());
            newUd.btc       = float.Parse(reader[3].ToString());
            newUd.eth       = float.Parse(reader[4].ToString());
            newUd.ltc       = float.Parse(reader[5].ToString());
            newUd.eur       = float.Parse(reader[6].ToString());

            ud = newUd;
        }
        
        // --- DISCONNECTION
        dbcon.Close();
        
        // --- SEND FEEDBACK
        return ud.pseudo != ""; // true = GRANTED | false = DENIED
    }

    void ReadUserData(IDbConnection conn)
    {
        // --- INIT COMMAND
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;

        // --- READ IN TABLE
        string query = "SELECT * FROM user_table WHERE pseudo = '" + Encrypt(ud.pseudo) + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        // --- USE BROUGHT DATA (& FILL USER DATA WITH)
        while (reader.Read())
        {
            userData newUd = new userData();

            newUd.id        = int.Parse(reader[0].ToString());
            newUd.pseudo    = Decrypt(reader[1].ToString());
            newUd.btc       = float.Parse(reader[3].ToString());
            newUd.eth       = float.Parse(reader[4].ToString());
            newUd.ltc       = float.Parse(reader[5].ToString());
            newUd.eur       = float.Parse(reader[6].ToString());

            ud = newUd;
        }
    }
    
    void UpdateNftValuesByReference(IDbConnection conn, string reference, float price, int id)
    {
        float btc = 0, eth = 0, ltc = 0, eur = 0;

        // --- SET VALUES BY REFERENCE
        if (reference == "BTC")
        {
            btc = price;
            eth = BtcToEth(price);
            ltc = BtcToLtc(price);
            eur = BtcToEur(price);
        }
        else if (reference == "ETH")
        {
            btc = EthToBtc(price);
            eth = price;
            ltc = EthToLtc(price);
            eur = EthToEur(price);
        }
        else if (reference == "LTC")
        {
            btc = LtcToBtc(price);
            eth = LtcToEth(price);
            ltc = price;
            eur = LtcToEur(price);
        }
        else
        {
            btc = EurToBtc(price);
            eth = EurToEth(price);
            ltc = EurToLtc(price);
            eur = price;
        }
        
        IDbCommand cmnd_update = conn.CreateCommand();
        string query = "UPDATE nft_table SET btc = '" + CTP(btc) + "' WHERE id = '" + id +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        query = "UPDATE nft_table SET eth = '" + CTP(eth) + "' WHERE id = '" + id +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        query = "UPDATE nft_table SET ltc = '" + CTP(ltc) + "' WHERE id = '" + id +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
        
        query = "UPDATE nft_table SET eur = '" + CTP(eur) + "' WHERE id = '" + id +"'";
        cmnd_update.CommandText = query;
        cmnd_update.ExecuteNonQuery();
    }

    void DropTables(IDbConnection conn)
    {
        IDbCommand dbcmd;
        dbcmd = conn.CreateCommand();

        dbcmd.CommandText = "DROP TABLE IF EXISTS nft_table";
        dbcmd.ExecuteNonQuery();
        dbcmd.CommandText = "DROP TABLE IF EXISTS user_table";
        dbcmd.ExecuteNonQuery();
    }

    string Encrypt(string input)
    {
        byte[] d = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider()
                       {Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(d, 0, d.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }
    
    string Decrypt(string input)
    {
        byte[] d = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider()
                       {Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7})
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(d, 0, d.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }
    
    

    // --- CONVERSIONS
    string CTP(float f)
    {
        string s = f.ToString();
        s = s.Replace(',', '.');
        return s;
    }
    
    public float EurToBtc(float eur)
    {
        float EUR = gcd.cryptoList[3].value;
        float BTC = gcd.cryptoList[0].value;
        return eur * BTC / EUR;
    }
    public float EurToEth(float eur)
    {
        float EUR = gcd.cryptoList[3].value;
        float ETH = gcd.cryptoList[1].value;
        return eur * ETH / EUR;
    }
    public float EurToLtc(float eur)
    {
        float EUR = gcd.cryptoList[3].value;
        float LTC = gcd.cryptoList[2].value;
        return eur * LTC / EUR;
    }
    
    public float BtcToEth(float btc)
    {
        float BTC = gcd.cryptoList[0].value;
        float ETH = gcd.cryptoList[1].value;
        return btc * ETH / BTC;
    }
    public float BtcToLtc(float btc)
    {
        float BTC = gcd.cryptoList[0].value;
        float LTC = gcd.cryptoList[2].value;
        return btc * LTC / BTC;
    }
    public float BtcToEur(float btc)
    {
        float BTC = gcd.cryptoList[0].value;
        float EUR = gcd.cryptoList[3].value;
        return btc * EUR / BTC;
    }

    public float LtcToEth(float ltc)
    {
        float LTC = gcd.cryptoList[2].value;
        float ETH = gcd.cryptoList[1].value;
        return ltc * ETH / LTC;
    }
    public float LtcToBtc(float ltc)
    {
        float LTC = gcd.cryptoList[2].value;
        float BTC = gcd.cryptoList[0].value;
        return ltc * BTC / LTC;
    }
    public float LtcToEur(float ltc)
    {
        float LTC = gcd.cryptoList[2].value;
        float EUR = gcd.cryptoList[3].value;
        return ltc * EUR / LTC;
    }

    public float EthToBtc(float eth)
    {
        float ETH = gcd.cryptoList[1].value;
        float BTC = gcd.cryptoList[0].value;
        return eth * BTC / ETH;
    }
    public float EthToLtc(float eth)
    {
        float ETH = gcd.cryptoList[1].value;
        float LTC = gcd.cryptoList[2].value;
        return eth * LTC / ETH;
    }
    public float EthToEur(float eth)
    {
        float ETH = gcd.cryptoList[1].value;
        float EUR = gcd.cryptoList[3].value;
        return eth * EUR / ETH;
    }
}
