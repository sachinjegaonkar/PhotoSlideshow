using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSlideshow
{
    static class Program
    {
        public static PhotoCollection ReadFromInputFile(string inputFile)
        {
            long N;

            List<Photo> photos = new List<Photo>();

            if (!File.Exists(inputFile))
            {
                throw new Exception("Cannot find the input file.");
            }

            var fileStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            if (fileStream == null)
            {
                throw new Exception("Cannot open the input file.");
            }

            using (var streamReader = new StreamReader(fileStream))
            {
                var parameters = streamReader.ReadLine().Split(' ');
                long.TryParse(parameters[0], out N);

                if (!(1 <= N && N <= Math.Pow(10, 5)))
                {
                    throw new System.Exception("The input parameters are not in a valid range.");
                }

                for (long i = 0; i < N; ++i)
                {
                    char type; int tagCount;
                    Photo photo = new Photo();

                    var photoDescription = streamReader.ReadLine().Split(' ');
                    char.TryParse(photoDescription[0], out type);
                    photo.type = (type == 'H') ? PhotoType.Horizontal : PhotoType.Vertical;
                    int.TryParse(photoDescription[1], out tagCount);
                    photo.tagCount = tagCount;
                    if (!(1 <= tagCount && tagCount <= 100))
                    {
                        throw new System.Exception("The input parameters are not in a valid range.");
                    }
                    photo.tags = new string[tagCount];
                    for (int t = 2, p = 0; t < photoDescription.Length; t++, p++)
                        photo.tags[p] = photoDescription[t];

                    photos.Add(photo);
                }
            }

            if ((long)photos.Count != N)
            {
                throw new System.Exception("The pizza data is not in a valid range.");
            }

            return new PhotoCollection(N, photos);
        }

        public static void WriteToOutputFile(Slideshow slideShow, string outputFile)
        {
            if (slideShow.slides == null)
                return;

            var fileStream = new FileStream(outputFile, FileMode.CreateNew, FileAccess.Write);
            if (fileStream == null)
            {
                throw new Exception("Cannot open the output file.");
            }

            using (var streamWriter = new StreamWriter(fileStream))
            {
                streamWriter.WriteLine(slideShow.slideCount);

                foreach (var slide in slideShow.slides)
                {
                    streamWriter.WriteLine(String.Join(" ", slide));
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Write("Usage: PhotoSlideshow <input_file_path> <output_file_path>");
                Console.Write("\n");
                return;
            }

            try
            {
                PhotoCollection collection = ReadFromInputFile(args[0]);
                var slideShow = collection.MakeSlideshow();
                WriteToOutputFile(slideShow, args[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return;
        }
    }
}
