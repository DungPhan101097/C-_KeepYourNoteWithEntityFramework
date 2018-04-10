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
    public abstract class Xl_TextNote_Observerble
    {
        public List<Xl_TextNote_Observer> clientLst = new List<Xl_TextNote_Observer>();

        public void registerNote(Xl_TextNote_Observer ins)
        {
            clientLst.Add(ins);
        }

        public void changeUI_Note()
        {
            foreach(var obs in clientLst)
            {
                obs.refreshUINote();
            }
        }
    }

    public interface Xl_TextNote_Observer
    {
        void refreshUINote();
    }

    public class XuLy_DuLieu_TextNote : Xl_TextNote_Observerble
    {
    
        NoteDBDung myModel;

        public XuLy_DuLieu_TextNote(NoteDBDung model)
        {
            myModel = model;
        }

        public TEXTNOTE insertTextNote(TEXTNOTE newTEXTNOTE)
        {
            TEXTNOTE newTextNote= myModel.TEXTNOTEs.Add(newTEXTNOTE);
            myModel.SaveChanges();

            changeUI_Note();

            return newTextNote;
        }

        public bool modifyTextNote(TEXTNOTE modifiedTextNote, TEXTNOTE newTextNote)
        {
            DbSet<TEXTNOTE> listTextNotes = myModel.TEXTNOTEs;
            bool flagReturn = false;

            var textNote = (from note in listTextNotes
                                where note.IdNote == modifiedTextNote.IdNote
                                select note).FirstOrDefault<TEXTNOTE>();
            if (textNote != null)
            {
                textNote.assignTextNote(newTextNote);
                flagReturn =  true;
            }

            // Change UI
            myModel.SaveChanges();

            changeUI_Note();

            return flagReturn;
        }

        public bool removeTextNote(TEXTNOTE removedTEXTNOTE)
        {
            DbSet<TEXTNOTE> listTextNotes = myModel.TEXTNOTEs;
            bool flagReturn = false;

            var textNote = (from note in listTextNotes
                            where note.IdNote == removedTEXTNOTE.IdNote
                            select note).FirstOrDefault<TEXTNOTE>();
            // Delete it from memory.
            if (textNote != null)
            {
                myModel.TEXTNOTEs.Remove(textNote);
                flagReturn =  true;
            }

            // Change UI
            myModel.SaveChanges();

            changeUI_Note();

            return flagReturn;
        }

        public List<TEXTNOTE> getAllTextNote()
        {
            List<TEXTNOTE> listNote = new List<TEXTNOTE>();
            DbSet<TEXTNOTE> dbNote = myModel.TEXTNOTEs;

            foreach (TEXTNOTE note in dbNote)
            {
                listNote.Add(note);
            }
            return listNote;
            //return myModel.TEXTNOTEs;
        }

        public IQueryable<TEXTNOTE> statitistic5MostRecentTEXTNOTEs()
        {
            DbSet<TEXTNOTE> listTextNotes = myModel.TEXTNOTEs;
            // listTextNotes.OrderBy(item => item.mAccessTime);

            if (listTextNotes.Count() > 5)
            {
                return listTextNotes.Take(5);
            }
            return myModel.TEXTNOTEs;
        }

        public List<TEXTNOTE> findTextNotesByKeyWord(String keyWord)
        {
            DbSet<TEXTNOTE> listTextNotes = myModel.TEXTNOTEs;

            return (from note in listTextNotes
                    where note.mContent.Contains(keyWord) || note.mTitle.Contains(keyWord)
                    select note).ToList();
        }

        public TEXTNOTE getTextNoteByID(TEXTNOTE textNote)
        {
            if (textNote == null) return null;

            DbSet<TEXTNOTE> listTextNotes = myModel.TEXTNOTEs;

            var selectNote = (from note in listTextNotes
                              where note.IdNote == textNote.IdNote
                              select note);

            var firstNote = selectNote.First();
            return firstNote;
        }


        //public List<TEXTNOTE> getPreviewAllTextNote()
        //{
        //    List<TEXTNOTE> listTextNote = new List<TEXTNOTE>();
        //    DbSet<TAG> listTags = myModel.TAGs;

        //    foreach (TAG tag in listTags)
        //    {
        //        listTextNote.Concat<TEXTNOTE>(tag.getAllPreviewTextNote());
        //    }

        //    return listTextNote;
        //}

        public TEXTNOTE findNoteByID(int id)
        {
            TEXTNOTE result = null;
            DbSet<TEXTNOTE> dbNote = myModel.TEXTNOTEs;

            foreach (TEXTNOTE note in dbNote)
            {
                if (note.IdNote == id)
                {
                    result = note;
                    break;
                }
            }
            return result;

        }

        public void saveChangeTextNote(TEXTNOTE curNote, TEXTNOTE newNote)
        {
            curNote.assignTextNote(newNote);
        }

    }
}
