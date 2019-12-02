using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectMusicAnalyzer
{
    public class InfoSongsPlaylist
    {
        public string NameOfSong;
        public string ArtistOfSong;
        public string SongAlbum;
        public string GenreOfSong;
        public int Size;
        public int Time;
        public int Year;
        public int AmountOfPlays;

        public InfoSongsPlaylist(string name, string artist, string album, string genre, int size, int time, int year, int plays)
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

        public override string ToString()
        {
            return String.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}", NameOfSong, ArtistOfSong, SongAlbum, GenreOfSong, Size, Time, Year, AmountOfPlays);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string songReport = null;

            int i = 0;

            List<InfoSongsPlaylist> RowColumns = new List<InfoSongsPlaylist>();

            try
            {
                if (File.Exists($"SampleMusicPlaylist.txt"))
                {
                    StreamReader sr = new StreamReader($"SampleMusicPlaylist.txt");
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
                                Console.Write("$Row {index} Contains {strings.Length} values. There should be 8 in that row.\n");
                                break;
                            }
                            else
                            {
                                InfoSongsPlaylist dataTemp = new InfoSongsPlaylist((strings[0]), (strings[1]), (strings[2]),
                                (strings[3]), Int32.Parse(strings[4]), Int32.Parse(strings[5]), Int32.Parse(strings[6]),
                                Int32.Parse(strings[7]));
                                RowColumns.Add(dataTemp);
                            }
                        }

                        catch (Exception e)
                        {
                            Console.Write("Unable to read lines from the playlist data file");
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
            }

            try
            {
                InfoSongsPlaylist[] songs = RowColumns.ToArray();
                using (StreamWriter write = new StreamWriter("SampleMusicPlaylist.txt"))
                {
                    write.WriteLine("Report of this Music Analyst\n");

                    var SongsPlayed200Above = from song in songs where song.AmountOfPlays >= 200 select song;
                    songReport += "\nHere is the list of the songs that have been played at 200 times and above: \n";

                    if (SongsPlayed200Above.Count() > 0)
                    {
                        songReport += SongsPlayed200Above.Count();
                    }
                    else
                    {
                        songReport += "No information found.";
                    }


                    var SongsWithGenreAlternative = from song in songs where song.GenreOfSong == "Alternative" select song;
                    i = 0;

                    foreach (InfoSongsPlaylist song in SongsWithGenreAlternative)
                    {
                        i++;
                    }

                    songReport += "\nHere is the list of songs that are in the 'Alternative' genre: {i}\n \n";

                    var SongsWithGenreHipHopRap = from song in songs where song.GenreOfSong == "Hip-Hop/Rap" select song;
                    i = 0;
                    foreach (InfoSongsPlaylist song in SongsWithGenreHipHopRap)
                    {
                        i++;
                    }

                    songReport += $"Here's the information on how many songs are in the Hip-Hop/Rap genre: {i}\n";

                    var SongsWithFishbowlAlbum = from song in songs where song.SongAlbum == "Welcome to the Fishbowl" select song;
                    songReport += "Here are the songs that are under the Welcome to the Fishbowl Album: \n";
                    foreach (InfoSongsPlaylist songsPlaylist in SongsWithFishbowlAlbum)
                    {
                        songReport += songsPlaylist + "\n";
                    }

                    var SongsIn1970 = from song in songs where song.Year < 1970 select song;
                    songReport += "\nHere are the songs from earlier than 1970: \n \n";
                    foreach (InfoSongsPlaylist songsPlaylist in SongsIn1970)
                    {
                        songReport += songsPlaylist + "\n";
                    }

                    var NamesWith85Characters = from song in songs where song.NameOfSong.Length > 85 select song.NameOfSong;
                    songReport += "\nHere are the list of songs that are longer than 85 characters: \n";
                    foreach (string name in NamesWith85Characters)
                    {
                        songReport += name + "\n";
                    }

                    var SongLongestLength = from song in songs orderby song.Time descending select song;
                    songReport += "\nLongest Song: \n \n";
                    songReport += SongLongestLength.First();

                    write.Write(songReport);
                    write.Close();
                }

                Console.WriteLine("You have successfully created a music playlist report file.");
            }

            catch (Exception e)
            {
                Console.WriteLine("Report fiile can't be opened/written");
            }

            Console.ReadLine();
        }
    }
}

