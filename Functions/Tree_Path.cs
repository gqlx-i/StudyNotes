using System.IO;
using System.Text;
using System;
using StudyNotes.CustomAttribute;

namespace StudyNotes.Functions
{
    class Tree_Path
    {
        /// <summary>
        /// 展开一个路径下的所有文件
        /// </summary>
        /// <param name="path">目标路径</param>
        /// <param name="rank">初始目录级别,默认0</param>
        [Method(@"C:\Users\Lenovo\Desktop\halcon_test", 0)]
        public void Tree(string path, int rank = 0)
        {
            Console.WriteLine(GetRowString(path, rank, false));
            string[] paths = Directory.GetFileSystemEntries(path);
            foreach (var item in paths)
            {
                if (File.Exists(item))
                {
                    Console.WriteLine(GetRowString(item, rank));
                }
                else
                {
                    Tree(item, rank + 1);
                }
            }
        }

        /// <summary>
        /// 获取输出字符串
        /// </summary>
        /// <param name="path">目标路径</param>
        /// <param name="rank">目录级别</param>
        /// <param name="isFile">是否是文件,默认是文件</param>
        /// <returns></returns>
        private string GetRowString(string path, int rank, bool isFile = true)
        {
            //字符串拼接对象，也可以使用string.join()
            StringBuilder str_build = new StringBuilder();
            //每行最开始
            string row_head = " ";
            string row_content = "--";
            int cur_rank = rank - (isFile ? 0 : 1);
            //是否可以展开
            string expand_flag = isFile ? "-" : "+";
            //是否同级
            string rank_flag = "+";

            // 如果是根目录，去除不必要的前缀
            if (!isFile && rank == 0)
            {
                rank_flag = "";
                row_head = "";
                row_content = "";
            }

            str_build.Append(row_head);
            //循环添加样式
            while (cur_rank > 0)
            {
                str_build.Append("|   ");
                cur_rank -= 1;
            }
            str_build.Append(rank_flag)
                     .Append(row_content)
                     .Append(expand_flag)
                     .Append(GetLastWord(path));
            return str_build.ToString();
        }

        /// <summary>
        /// 返回目标对象名称
        /// </summary>
        /// <param name="path">目标路径</param>
        /// <param name="flag">是否添加文件夹标志,默认添加</param>
        /// <returns></returns>
        private string GetLastWord(string path, bool flag = true)
        {
            string tail = flag && Directory.Exists(path) ? "\\" : "";
            return Path.GetFileName(path) + tail;
        }
    }
}