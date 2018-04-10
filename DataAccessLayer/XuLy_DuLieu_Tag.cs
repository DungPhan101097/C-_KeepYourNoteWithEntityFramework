using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace DataAccessLayer
{
    public abstract class Xl_Tag_Observable
    {
        public List<Xl_Tag_Observer> clientLst = new List<Xl_Tag_Observer>();

        public void register(Xl_Tag_Observer ins)
        {
            clientLst.Add(ins);
        }

        public void changeUI()
        {
            foreach(var tagOb in clientLst)
            {
                tagOb.refreshUI();
            }
        }
    }

    public interface Xl_Tag_Observer
    {
        void refreshUI();
    }

    public class XuLy_DuLieu_Tag : Xl_Tag_Observable
    {
        NoteDBDung myModel;

        public XuLy_DuLieu_Tag(NoteDBDung model)
        {
            myModel = model;
        }

        public TAG insertTAG(TAG newTAG)
        {
            TAG newTag =  myModel.TAGs.Add(newTAG);

            // Change UI
            myModel.SaveChanges();

            changeUI();

            return newTag;
        }

        public TAG getTagByContent(string content)
        {
            DbSet<TAG> listTags = myModel.TAGs;

            var selectedTag = (from tag in listTags
                                where tag.mContent == content
                                select tag).FirstOrDefault<TAG>();
            return selectedTag;

        }

        public bool modifyTAG(TAG modifiedTAG, string content)
        {
            DbSet<TAG> listTags = myModel.TAGs;

            var myChangedTag = (from tag in listTags
                                where tag.IDTag == modifiedTAG.IDTag
                                select tag).FirstOrDefault<TAG>();
            if (myChangedTag != null)
            {
                myChangedTag.mContent = content;
                myChangedTag.mAccessTime = DateTime.Now;
            }
            // Change UI
            myModel.SaveChanges();

            changeUI();

            return true;
        }

        public bool removeTAG(TAG removedTAG)
        {
            DbSet<TAG> listTags = myModel.TAGs;
            
            var myRemovedTag = (from tag1 in listTags
                                where tag1.IDTag == removedTAG.IDTag
                                select tag1).First();
            // Delete it from memory.
            myModel.TAGs.Remove(myRemovedTag);

            myModel.SaveChanges();

            changeUI();

            return true;
        }

        public List<TAG> getAllTAG()
        {
            List<TAG> listTag = new List<TAG>();
            foreach (TAG tag in myModel.TAGs)
            {
                listTag.Add(tag);
            }
            return listTag;

            //return myModel.TAGs;

        }

        public IQueryable<TAG> statitistic5MostRecentTags()
        {
            DbSet<TAG> listTags = myModel.TAGs;
            var resultListTag = (from tag in listTags
                                 orderby tag.mAccessTime descending
                                 select tag).Take(5);

            return resultListTag;
        }

        public List<TAG> findTagsByKeyWord(String keyWord)
        {
            DbSet<TAG> listTags = myModel.TAGs;
            
           return (from tag in listTags
                    where tag.mContent.Contains(keyWord)
                    select tag).ToList();
        }

        

        //public List<TEXTNOTE> getAllPreviewTextNoteByTag(String contentTag)
        //{
        //    List<TEXTNOTE> listTextNote;
        //    DbSet<TAG> listTags = myModel.TAGs;

        //    TAG selectedTag = (from tag in listTags
        //                       where tag.mContent == contentTag
        //                       select tag).First<TAG>();
        //    listTextNote = selectedTag.getAllPreviewTextNote();

        //    return listTextNote;
        //}

        public List<TEXTNOTE> getAllTextNoteByTag(string contentTag)
        {
            List<TEXTNOTE> listTextNote;
            DbSet<TAG> listTags = myModel.TAGs;

            TAG selectedTag = (from tag in listTags
                               where tag.mContent == contentTag
                               select tag).First<TAG>();
            listTextNote = selectedTag.getAllTextNote();

            return listTextNote;
        }

        public List<DRAWINGNOTE> getAllPreviewDrawingNoteByTag(String contentTag)
        {
            List<DRAWINGNOTE> listDrawingNote = new List<DRAWINGNOTE>();
            

            return listDrawingNote;
        }

        public List<Tuple<TAG, int>> statisticNumberNotePerTag()
        {
            DbSet<TAG> listTags = myModel.TAGs;
            List<Tuple<TAG, int>> result = new List<Tuple<TAG, int>>();

            foreach (TAG tag in listTags)
            {
                Tuple<TAG, int> pair = new Tuple<TAG, int>(tag, tag.countNumberNote());
                result.Add(pair);
            }

            return result;
        }

        
       
    }
}
