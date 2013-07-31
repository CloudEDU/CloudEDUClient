using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudEDU.CourseStore
{
    class Lesson
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        private List<Resource> docs;
        private List<Resource> audios;
        private List<Resource> videos;

        public Lesson(int number, string title, string content)
        {
            Number = number;
            Title = title;
            Content = content;
            docs = new List<Resource>();
            audios = new List<Resource>();
            videos = new List<Resource>();
        }

        public List<Resource> GetDocList()
        {
            return docs;
        }

        public List<Resource> GetAudioList()
        {
            return audios;
        }

        public List<Resource> GetVideoList()
        {
            return videos;
        }
    }

    class Resource
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Type { get; set; }

        public Resource(string title, string uri, string type)
        {
            Title = title;
            Uri = uri;
            Type = type;
        }
    }
}
