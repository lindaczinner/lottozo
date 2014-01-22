using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using LottozoCore;
using LottozoCore.Helpers;
using Newtonsoft.Json;

namespace Lottoszamok.Utils
{
	public class StorageHelper
	{
		public void Save<T>(T dataToSave)
		{
			var stringData = JsonSerialize(dataToSave);

			if (String.IsNullOrEmpty(stringData))
				return;

			var fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
			using (var writer = new StreamWriter(new IsolatedStorageFileStream(Constants.SavedDataFileName, FileMode.OpenOrCreate, fileStorage)))
			{
				writer.Write(stringData);
			}
		}

		public T Load<T>()
			where T : class
		{
			var fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
			StreamReader reader = null;
			string stringData;

			try
			{
				reader = new StreamReader(new IsolatedStorageFileStream(Constants.SavedDataFileName, FileMode.Open, fileStorage));
				stringData = reader.ReadToEnd();
			}
			catch (IsolatedStorageException)
			{
				stringData = null;
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}

			return JsonDeserialize<T>(stringData);
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

				return null;

			}
		}
	}
}
