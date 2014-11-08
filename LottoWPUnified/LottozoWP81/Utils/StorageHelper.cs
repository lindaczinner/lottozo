using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using LottozoCore;
using LottozoCore.Helpers;
using LottozoCore.Interfaces;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;

namespace Lottozo.Utils
{
	public class StorageHelper
	{
		private readonly StorageFolder localFolder;

		public StorageHelper()
		{
			localFolder = ApplicationData.Current.LocalFolder;
		}

		public async void SaveAsync<T>(T dataToSave)
		{
			var stringData = JsonSerialize(dataToSave);

			if (String.IsNullOrEmpty(stringData))
				return;

			var dataFile = await localFolder.CreateFileAsync(Constants.SavedDataFileName, CreationCollisionOption.ReplaceExisting);

			using (var stream = await dataFile.OpenAsync(FileAccessMode.ReadWrite))
			{
				using (var dataWriter = new DataWriter(stream))
				{
					dataWriter.WriteString(stringData);
					await dataWriter.StoreAsync();
				}
			}
		}

		public async Task<T> LoadAsync<T>()
			where T : class
		{
			StorageFile dataFile;

			try
			{
				dataFile = await localFolder.GetFileAsync(Constants.SavedDataFileName);
			}
			catch (FileNotFoundException)
			{
				dataFile = null;
			}
			

			if (dataFile == null)
				return null;

			using (var stream = await dataFile.OpenReadAsync())
			{
				using (var dataReader = new DataReader(stream))
				{
					var length = (uint)stream.Size;
					await dataReader.LoadAsync(length);
					var stringData = dataReader.ReadString(length);
					return JsonDeserialize<T>(stringData);
				}
			}
		}

		private string JsonSerialize<T>(T objectToSave)
		{
			try
			{
				var jsonObj = JsonConvert.SerializeObject(objectToSave);
				return jsonObj;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>
					{
						{"objectToSave", objectToSave.ToString()}
					};

				LogHelper.LogException("Serialization exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;

			}
		}

		private T JsonDeserialize<T>(string deserializable)
			where T : class
		{
			try
			{
				var obj = JsonConvert.DeserializeObject<T>(deserializable);
				return obj;
			}
			catch (Exception ex)
			{
				var attributes = new Dictionary<String, String>
					{
						{"deserializable", deserializable}
					};

				LogHelper.LogException("Deserialization exception", ex, attributes);
				IoC.Get<IBugsenseHelper>().LogBugsenseException(ex, attributes);

				return null;

			}
		}
	}
}