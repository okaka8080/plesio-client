using UnityEditor;
using UnityEngine;
using System.IO;

//�N�����Ɏ��s
[InitializeOnLoad]
public class FolderKeeper : AssetPostprocessor
{

    //�L�[�p�[�̖��O
    public static readonly string keeperName = ".gitkeep";

    //�R���X�g���N�^�i�N�����ɌĂяo�����j
    static FolderKeeper()
    {
        //�������Ăяo��
        SetKeepers();
    }

    //�A�Z�b�g�X�V���Ɏ��s
    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetsPath)
    {
        SetKeepers();
    }

    //���j���[�ɃA�C�e����ǉ�
    [MenuItem("Tools/Set Keepers")]

    //�Ăяo���֐�
    public static void SetKeepers()
    {

        //�L�[�p�[��z�u����
        CheckKeeper("Assets");

        //�f�[�^�x�[�X�����t���b�V������
        AssetDatabase.Refresh();

    }

    //�L�[�p�[��z�u����֐�
    public static void CheckKeeper(string path)
    {

        //�f�B���N�g���p�X�̔z��
        string[] directories = Directory.GetDirectories(path);
        //�t�@�C���p�X�̔z��
        string[] files = Directory.GetFiles(path);
        //�L�[�p�[�̔z��
        string[] keepers = Directory.GetFiles(path, keeperName);

        //�f�B���N�g�������邩�ǂ���
        bool isDirectoryExist = 0 < directories.Length;
        //(�L�[�p�[�ȊO��)�t�@�C�������邩�ǂ���
        bool isFileExist = 0 < (files.Length - keepers.Length);
        //�L�[�p�[�����邩�ǂ���
        bool isKeeperExist = 0 < keepers.Length;

        //�f�B���N�g�����t�@�C�����Ȃ�������
        if (!isDirectoryExist && !isFileExist)
        {
            //�L�[�p�[���Ȃ�������
            if (!isKeeperExist)
            {
                //�L�[�p�[���쐬
                File.Create(path + "/" + keeperName).Close();
                Debug.Log("Keeper Created : " + path);
            }
            return;
        }
        //�f�B���N�g�����t�@�C������������
        else
        {
            //�L�[�p�[����������
            if (isKeeperExist)
            {
                //�L�[�p�[���폜
                File.Delete(path + "/" + keeperName);
                Debug.Log("Keeper Deleted : " + path);
            }
        }

        //����ɐ[���K�w��T��
        foreach (var directory in directories)
        {
            CheckKeeper(directory);
        }
    }
}


