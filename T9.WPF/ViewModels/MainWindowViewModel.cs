using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using T9.DAL.DbModels;
using T9.Services.Common;
using T9.WPF.Helpers;
using System.Data.Entity;

namespace T9.WPF.ViewModels
{
    class MainWindowViewModel : NotifyPropertyChanged
    {
        #region Private Definitions
        string m_text;
        ObservableCollection<string> m_similarWords;
        SimilarWordsService m_similarWordsService;
        TextConverter m_textConverter;
        int m_selectionStart;
        string m_selectedText;
        #endregion

        public MainWindowViewModel()
        {
            InitCommands();

            m_similarWordsService = new SimilarWordsService();
            m_textConverter = new TextConverter();

            SelectionStartChanged += OnSelectionStartChanged;
        }

        public string Text
        {
            get => m_text;
            set
            {
                m_text = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> SimilarWords
        {
            get => m_similarWords;
            set
            {
                m_similarWords = value;
                OnPropertyChanged();
            }
        }
        public int SelectionStart
        {
            get => m_selectionStart;
            set
            {
                m_selectionStart = value;
                OnPropertyChanged();
                SelectionStartChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public string SelectedText
        {
            get => m_selectedText;
            set
            {
                m_selectedText = value;
                OnPropertyChanged();
            }
        }

        public ICommand CallOnScreenKeyboardCommand { get; set; }
        public ICommand OnSimilarWordButtonClickCommand { get; set; }
        public ICommand OnSearchButtonClickCommand { get; set; }
        public ICommand AddWordToVocabularyCommand { get; set; }

        public event EventHandler SelectionStartChanged;

        void InitCommands()
        {
            CallOnScreenKeyboardCommand = new RelayCommand(CallOnScreenKeyboard);
            OnSimilarWordButtonClickCommand = new RelayCommand<string>(OnSimilarWordButtonClickAsync);
            OnSearchButtonClickCommand = new RelayCommand(OnSearchButtonClick);
            AddWordToVocabularyCommand = new RelayCommand(AddWordToVocabulary);
        }

        void CallOnScreenKeyboard()
        {
            Process.Start("osk.exe");
        }
        async void OnSimilarWordButtonClickAsync(string word)
        {
            ReplaceTargetWord(word);
            SimilarWords.Clear();

            using (var context = new T9_VocabularyContext())
            {
                Word target = await context.Words.FirstOrDefaultAsync(w => w.WordName == word);
                target.UsesCount++;
                await context.SaveChangesAsync();
            }
        }
        void OnSearchButtonClick()
        {
            Process.Start($"http://google.com/search?q={Text}");
        }
        async void AddWordToVocabulary()
        {
            using (var context = new T9_VocabularyContext())
            {
                string targetWord = SelectedText;

                context.Words.Add(new Word { WordName = SelectedText });
                await context.SaveChangesAsync();
            }
        }

        async Task GetSimilarWordsAsync()
        {
            string word = GetTargetWord();
            var similarWords = await m_similarWordsService.GetSimilarWordsAsync(word);

            // If nothing was found.
            if (similarWords.Count == 0)
            {
                word = ConvertToAnotherLayout(word);
                similarWords = await m_similarWordsService.GetSimilarWordsAsync(word);
            }

            SimilarWords = new ObservableCollection<string>(similarWords);
        }
        string ConvertToAnotherLayout(string word)
        {
            string language = InputLanguageManager.Current.CurrentInputLanguage.TwoLetterISOLanguageName;

            if (language == "en")
            {
                return m_textConverter.ConvertToRussianLayout(word);
            }
            if (language == "ru")
            {
                return m_textConverter.ConvertToEnglishLayout(word);
            }
            throw new Exception($"Language {language} is not supported");
        }
        async void OnSelectionStartChanged(object sender, EventArgs e)
        {
            try
            {
                await GetSimilarWordsAsync();
            }
            catch { }
        }

        string GetTargetWord()
        {
            int startIndex = GetTargetWordStartIndex();

            string word = Text.Substring(startIndex, SelectionStart - startIndex);
            return word;
        }
        void ReplaceTargetWord(string newWord)
        {
            int startIndex = GetTargetWordStartIndex();
            int count = SelectionStart - startIndex;

            Text = Text.ReplaceAt(startIndex, count, newWord);

            SelectionStart = startIndex + newWord.Length;
        }
        int GetTargetWordStartIndex()
        {
            return Text.LastIndexOf(' ', SelectionStart - 1) + 1;
        }
    }
}
