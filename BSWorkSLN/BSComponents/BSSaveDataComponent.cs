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
        public class BSSaveDataComponent : BSBaseComponent, IConfigurable
        {
            public BSSaveDataComponent(Rectangle iOnScreenRectangle)
                : base(iOnScreenRectangle)
            {
            }

            public override string ComponentType
            {
                get { return "O"; }
            }

            public override void Start()
            {
                CheckIfInputsAreProvided();
                BSDataObject objToSave;
                Inputs.TryPop(out objToSave);
                BSRepository.SaveDataObjectToFile(objToSave, fileToSavePath);
            }

            public override void ShowSettingsForm()
            {
                if (null == aForm)
                    aForm = new BSSaveDataComponentForm(this);
                aForm.Show();
            }

            public void SetValueForKey(string iKey, object iValue)
            {
                switch (iKey)
                {
                    case "fileToSavePath":
                        fileToSavePath = (iValue as string);
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
                return fileToSavePath != null;
            }

            public override string WhatsWrong()
            {
                if (fileToSavePath == null)
                    return "Не задане ім'я файлу, куди зберігати дані для компонента 'Збереження даних'.";
                else
                    return "Компонент 'Збереження даних' налаштований коректно.";
            }

            private string fileToSavePath;
            private BSSaveDataComponentForm aForm = null;
        }
    }
}
