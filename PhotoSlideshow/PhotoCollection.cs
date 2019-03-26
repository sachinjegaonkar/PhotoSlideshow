using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSlideshow
{
    public enum PhotoType
    {
        Horizontal,
        Vertical
    }

    public class Photo
    {
        public PhotoType type;
        public int tagCount;
        public string[] tags;
        public bool addedInSlideshow = false;
    }

    public class Slideshow
    {
        public long slideCount;
        public List<long[]> slides;
    }

    public class PhotoCollection
    {
        long count;
        List<Photo> Photos;

        private PhotoCollection()
        {
        } // default constructor

        public PhotoCollection(long photoCount, List<Photo> photos)
        {
            this.count = photoCount;
            this.Photos = photos;
        }

        public Slideshow MakeSlideshow()
        {
            Slideshow slideshow = new Slideshow();

            slideshow.slides = new List<long[]>();

            for (int i = 0; i < Photos.Count; i++)
            {
                if (Photos[i].addedInSlideshow && Photos[i].type != PhotoType.Vertical) continue;

                if (Photos[i].type == PhotoType.Horizontal ||
                    (Photos[i].type == PhotoType.Vertical && !Photos[i].addedInSlideshow))
                {
                    Photos[i].addedInSlideshow = true;

                    Photo neighbor;
                    FindHorizontalNeighboringSlide(Photos[i], out neighbor);
                    if (neighbor != null)
                    {
                        if (Photos[i].type == PhotoType.Vertical)
                            slideshow.slides.Add(new long[] { i, Photos.IndexOf(neighbor) });
                        else
                            slideshow.slides.Add(new long[] { Photos.IndexOf(neighbor) });
                        Photos[Photos.IndexOf(neighbor)].addedInSlideshow = true;
                    }
                    else
                    {
                        Photos[i].addedInSlideshow = false;
                    }
                }
                else
                {
                    Photos[i].addedInSlideshow = true;

                    Photo neighbor;
                    FindVerticalNeighboringSlide(Photos[0], out neighbor);
                    if (neighbor != null)
                    {
                        if (neighbor.type == PhotoType.Horizontal)
                        slideshow.slides.Add(new long[] { Photos.IndexOf(neighbor) });
                        else
                            slideshow.slides.Add(new long[] { i, Photos.IndexOf(neighbor) });

                        Photos[Photos.IndexOf(neighbor)].addedInSlideshow = true;
                    }
                    else
                    {
                        Photos[i].addedInSlideshow = false;
                    }
                }
            }

            slideshow.slideCount = slideshow.slides.Count;

            return slideshow;
        }

        private void FindHorizontalNeighboringSlide(Photo slide, out Photo neighbor)
        {
            // Horizontal or vertical photo
            neighbor = null;

            /*
             * the number of common tags between S i and S i+1
             * the number of tags in S i but not in S i+1
             * the number of tags in S i+1 but not in S i .
             */

            neighbor = Photos.Where(p =>
            {
                if (p.addedInSlideshow) return false;
                int commonTags = slide.tags.Count(x => p.tags.Contains(x));
                int tagsInSiButNotInSii = slide.tags.Count(x => !p.tags.Contains(x));
                int tagsInSiiButNotInSi = p.tags.Count(pp => !slide.tags.Contains(pp));

                return (commonTags > 1 && tagsInSiButNotInSii > 1 && tagsInSiiButNotInSi > 1);
            }).FirstOrDefault();

            // neighbor;
        }


        private void FindVerticalNeighboringSlide(Photo slide, out Photo neighbor)
        {
            neighbor = null;

            /*
             * the number of common tags between S i and S i+1
             * the number of tags in S i but not in S i+1
             * the number of tags in S i+1 but not in S i .
             */

            neighbor = Photos.Where(p =>
            {
                if (p.addedInSlideshow || p.type == PhotoType.Horizontal) return false;
                int commonTags = slide.tags.Count(x => p.tags.Contains(x));
                int tagsInSiButNotInSii = slide.tags.Count(x => !p.tags.Contains(x));
                int tagsInSiiButNotInSi = p.tags.Count(pp => !slide.tags.Contains(pp));

                return (commonTags > 1 && tagsInSiButNotInSii > 1 && tagsInSiiButNotInSi > 1);
            }).FirstOrDefault();

            // neighbor; anotherNeighbor;
        }
    }
}
