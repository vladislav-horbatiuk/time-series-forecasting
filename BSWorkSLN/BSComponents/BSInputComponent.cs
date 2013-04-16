using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using BSWork;
using BSWork.ComponentsSettingsForms;
using BSWork.ComponentsConfiguration;
using DataObjectsSupply;
using BSWork.DataObjects;

namespace BSWork
{
    namespace BSComponents
    {
        public class BSInputComponent : BSBaseComponent, IConfigurable
        {
            public BSInputComponent(Rectangle iOnScreenRectangle)
                : base(iOnScreenRectangle)
            {
            }

            public override string ComponentType
            {
                get { return "I"; }
            }

            public override void Start()
            {
                try
                {
                    Output = BSRepository.DataObjectFromFile(sourceFilePath);
                }
                catch (FormatException exc)
                {
                    throw new IncorrectInputFileException("Файл, заданий у якості джерела даних для елемента 'Вхідні дані' не відповідає потрібному формату.");
                }
            }

            public override void ShowSettingsForm()
            {
                if (null == aForm)
                    aForm = new InputComponentForm(this);
                aForm.Show();
            }

            public void SetValueForKey(string iKey, object iValue)
            {
                switch (iKey)
                {
                    case "sourceFilePath":
                        sourceFilePath = (iValue as string);
                        break;
                    default:
                        throw new ArgumentException();
                }
            }

            public object GetValueForKey(string iKey)
            {
                throw new NotImplementedException();
            }

            public override bool IsReady()
            {
                return sourceFilePath != null && File.Exists(sourceFilePath);
            }

            public override string WhatsWrong()
            {
                if (sourceFilePath == null || !File.Exists(sourceFilePath))
                    return "Не задані вхідні дані для компонента 'вхідні дані', або задане некоректне ім'я файлу.";
                else
                    return "Компонент 'Вхідні дані' налаштований коректно.";
            }

            private string sourceFilePath;
            private InputComponentForm aForm = null;
        }

        public class IncorrectInputFileException : UserException
        {
            public IncorrectInputFileException(string iMessage)
                : base(iMessage)
            {
            }
        }
    }
}
