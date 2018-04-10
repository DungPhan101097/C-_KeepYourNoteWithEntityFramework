using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace ViewNoteWindow.callbacks
{
    public interface MainWindowTagClickCallback
    {
        void onTagClick(TAG selectedTag);
    }
}
