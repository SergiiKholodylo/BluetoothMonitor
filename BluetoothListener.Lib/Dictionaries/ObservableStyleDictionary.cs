using System;
using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace BluetoothListener.Lib.Dictionaries
{
    public class ObservableStyleDictionary : ObservableSortedDictionary<string, Style>
    {
        #region constructors

        #region public

        public ObservableStyleDictionary() : base(new KeyComparer()) 
        {
        }

        #endregion public

        #endregion constructors

        #region key comparer class

        private class KeyComparer : IComparer<DictionaryEntry>
        {
            public int Compare(DictionaryEntry entry1, DictionaryEntry entry2)
            {
                return string.Compare((string)entry1.Key, (string)entry2.Key, StringComparison.CurrentCultureIgnoreCase);
            }
        }

        #endregion key comparer class

    }
}
