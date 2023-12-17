using UnityEditor;
using UnityEngine;
using System.IO;

//起動時に実行
[InitializeOnLoad]
public class FolderKeeper : AssetPostprocessor
{

    //キーパーの名前
    public static readonly string keeperName = ".gitkeep";

    //コンストラクタ（起動時に呼び出される）
    static FolderKeeper()
    {
        //処理を呼び出す
        SetKeepers();
    }

    //アセット更新時に実行
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetsPath)
    {
        SetKeepers();
    }

    //メニューにアイテムを追加
    [MenuItem("Tools/Set Keepers")]

    //呼び出す関数
    public static void SetKeepers()
    {

        //キーパーを配置する
        CheckKeeper("Assets");

        //データベースをリフレッシュする
        AssetDatabase.Refresh();

    }

    //キーパーを配置する関数
    public static void CheckKeeper(string path)
    {

        //ディレクトリパスの配列
        string[] directories = Directory.GetDirectories(path);
        //ファイルパスの配列
        string[] files = Directory.GetFiles(path);
        //キーパーの配列
        string[] keepers = Directory.GetFiles(path, keeperName);

        //ディレクトリがあるかどうか
        bool isDirectoryExist = 0 < directories.Length;
        //(キーパー以外の)ファイルがあるかどうか
        bool isFileExist = 0 < (files.Length - keepers.Length);
        //キーパーがあるかどうか
        bool isKeeperExist = 0 < keepers.Length;

        //ディレクトリもファイルもなかったら
        if (!isDirectoryExist && !isFileExist)
        {
            //キーパーがなかったら
            if (!isKeeperExist)
            {
                //キーパーを作成
                File.Create(path + "/" + keeperName).Close();
                Debug.Log("Keeper Created : " + path);
            }
            return;
        }
        //ディレクトリかファイルがあったら
        else
        {
            //キーパーがあったら
            if (isKeeperExist)
            {
                //キーパーを削除
                File.Delete(path + "/" + keeperName);
                Debug.Log("Keeper Deleted : " + path);
            }
        }

        //さらに深い階層を探索
        foreach (var directory in directories)
        {
            CheckKeeper(directory);
        }
    }
}


