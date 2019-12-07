using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectMusicAnalyzer
{
    public class Song
    {
        public string NameOfSong;
        public string ArtistOfSong;
        public string SongAlbum;
        public string GenreOfSong;
        public int Size;
        public int Time;
        public int Year;
        public int AmountOfPlays;

        public Song(string name, string artist, string album, string genre, int size, int time, int year, int plays)
        {
            NameOfSong = name;
            ArtistOfSong = artist;
            SongAlbum = album;
            GenreOfSong = genre;
            Size = size;
            Time = time;
            Year = year;
            AmountOfPlays = plays;
        }

        override public string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", NameOfSong, ArtistOfSong, SongAlbum, GenreOfSong, Size, Time, Year, AmountOfPlays);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string report = "";

            int i;

            List<Song> RowColumns = new List<Song>();

            try
            {
                if (File.Exists(args[0]))
                {
                    StreamReader sr = new StreamReader(args[1]);
                    i = 0;

                    string line = sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        i++;

                        try
                        {
                            string[] strings = line.Split('\t');

                            if (strings.Length < 8)
                            {
                                Console.Write("Record does not have the correct number of elements\n");
                                Console.WriteLine($"Row {i} Contains {strings.Length} values. There should be 8 in that row.\n");
                                break;
                            }
                            else
                            {
                                Song dataTemp = new Song((strings[0]), (strings[1]), (strings[2]),
                                (strings[3]), Int32.Parse(strings[4]), Int32.Parse(strings[5]), Int32.Parse(strings[6]),
                                Int32.Parse(strings[7]));
                                RowColumns.Add(dataTemp);
                            }
                        }

                        catch (Exception e)
                        {
                            Console.Write("Unable to read lines from the playlist data file");
                            Console.WriteLine(e.Message);
                            break;
                        }
                        sr.Close();
                    }
                }
                else
                {
                    Console.WriteLine("Sorry, we can't seem to find the text file for your Music Playlist.");
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Unable to open the playlist data file...");
                Console.WriteLine(e.Message);
            }

            try
            {
                Song[] songs = RowColumns.ToArray();
                using (StreamWriter write = new StreamWriter("ReportMusic.txt"))
                {
                    write.WriteLine("Report of this Music Analyst\n");

                    var SongsPlayed200Above = from song in songs where song.AmountOfPlays >= 200 select song;
                    report += "\nHere is the list of the songs that have been played at 200 times and above: \n";

                    if (SongsPlayed200Above.Count() > 0)
                    {
                        report += SongsPlayed200Above.Count();
                    }
                    else
                    {
                        report += "No information found.";
                    }


                    var SongsWithGenreAlternative = from song in songs where song.GenreOfSong == "Alternative" select song;
                    i = 0;

                    foreach (Song song in SongsWithGenreAlternative)
                    {
                        i++;
                    }

                    report += $"\nHere is the list of songs that are in the 'Alternative' genre: {i}\n \n";

                    var SongsWithGenreHipHopRap = from song in songs where song.GenreOfSong == "Hip-Hop/Rap" select song;
                    i = 0;
                    foreach (Song song in SongsWithGenreHipHopRap)
                    {
                        i++;
                    }

                    report += $"Here's the information on how many songs are in the Hip-Hop/Rap genre: {i}\n";

                    var SongsWithFishbowlAlbum = from song in songs where song.SongAlbum == "Welcome to the Fishbowl" select song;
                    report += "Here are the songs that are under the Welcome to the Fishbowl Album: \n";
                    foreach (Song songsPlaylist in SongsWithFishbowlAlbum)
                    {
                        report += songsPlaylist + "\n";
                    }

                    var SongsIn1970 = from song in songs where song.Year < 1970 select song;
                    report += "\nHere are the songs from earlier than 1970: \n \n";
                    foreach (Song songsPlaylist in SongsIn1970)
                    {
                        report += songsPlaylist + "\n";
                    }

                    var NamesWith85Characters = from song in songs where song.NameOfSong.Length > 85 select song.NameOfSong;
                    report += "\nHere are the list of songs that are longer than 85 characters: \n";
                    foreach (string name in NamesWith85Characters)
                    {
                        report += name + "\n";
                    }

                    var SongLongestLength = from song in songs orderby song.Time descending select song;
                    report += "\nLongest Song: \n \n";
                    report += SongLongestLength.First();

                    write.Write(report);
                    write.Close();
                }

                Console.WriteLine("You have successfully created a music playlist report file.");
            }

            catch (Exception e)
            {
                Console.WriteLine("Report file can't be opened/written");
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}