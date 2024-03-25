using System;
using System.Linq;
using System.Collections.Generic;

namespace Part01
{
    internal class Program
    {
        public class Track
        {
            private string name;
            private string artistName;
            private string albumName;
            private int duration; //duration in seconds

            public Track(string name, string artistName, string albumName, int duration)
            {
                SetName(name);
                SetArtistName(artistName);
                SetAlbumName(albumName);
                SetDuration(duration);
            }

            public string GetName() { return name; }
            public string GetArtistName() { return artistName; }
            public string GetAlbumName() { return albumName; }
            public int GetDuration() { return duration; }

            /* setters below throw exceptions for invalid input. See document */
            public void SetName(string name)
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Name cannot be null or empty.");
                this.name = name;
            }

            public void SetArtistName(string artistName)
            {
                if (string.IsNullOrEmpty(artistName))
                    throw new ArgumentException("Artist name cannot be null or empty.");
                this.artistName = artistName;
            }

            public void SetAlbumName(string albumName)
            {
                if (string.IsNullOrEmpty(albumName))
                    throw new ArgumentException("Album name cannot be null or empty.");
                this.albumName = albumName;
            }

            public void SetDuration(int duration)
            {
                if (duration < 1)
                    throw new ArgumentException("Duration must be at least 1 second.");
                this.duration = duration;
            }
        }

        public class DNode
        {
            protected Track song; //each node holds a song
            protected DNode next, prev; //pointers to next and prev nodes

            //Constructor that creates a node
            public DNode(Track t, DNode p, DNode n)
            {
                song = t;
                next = n;
                prev = p;
            }

            public Track GetTrack() { return song; }
            public DNode GetPrev() { return prev; }
            public DNode GetNext() { return next; }
            public void SetTrack(Track t) { song = t; }
            public void SetPrev(DNode p) { prev = p; }
            public void SetNext(DNode n) { next = n; }
        }

        public class DoublyLinkedList
        {
            protected int size;
            protected DNode header, tail;

            public DoublyLinkedList()
            {
                size = 0; //initial size of list 
                header = new DNode(null, null, null); //header points to null
                tail = new DNode(null, header, null); //tail's prev node is header (null)
                header.SetNext(tail); //header's next points to to tail
            }

            public int Size() { return size; } //return size of list
            public bool IsEmpty() { return size == 0; } //return true if list is empty, false otherwise

            public DNode GetFirst()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("List is empty.");
                return header.GetNext();
            }

            public DNode GetLast()
            {
                if (IsEmpty())
                    throw new InvalidOperationException("List is empty.");
                return tail.GetPrev();
            }

            public DNode GetPrev(DNode v)
            {
                if (v == header)
                    throw new ArgumentException("Current node is header.");
                return v.GetPrev();
            }

            public DNode GetNext(DNode v)
            {
                if (v == tail)
                    throw new ArgumentException("Current node is tail.");
                return v.GetNext();
            }

            public void AddBefore(DNode v, DNode z)
            {
                DNode u = GetPrev(v);
                z.SetPrev(u);
                z.SetNext(v);
                v.SetPrev(z);
                u.SetNext(z);
                size++;
            }

            public void AddAfter(DNode v, DNode z)
            {
                DNode w = GetNext(v);
                z.SetPrev(v);
                z.SetNext(w);
                w.SetPrev(z);
                v.SetNext(z);
                size++;
            }

            public void AddFirst(DNode v)
            {
                AddAfter(header, v);
            }

            public void AddLast(DNode v)
            {
                AddBefore(tail, v);
            }

            public void Remove(DNode v)
            {
                if (v == header || v == tail)
                    throw new InvalidOperationException("Cannot remove header or tail.");
                DNode u = GetPrev(v);
                DNode w = GetNext(v);
                u.SetNext(w);
                w.SetPrev(u);
                size--;
            }

            public bool HasPrev(DNode v)
            {
                return v != header;
            }

            public bool HasNext(DNode v)
            {
                return v != tail;
            }
        }
        
        public class Playlist
        {
            private string name;
            private int count;
            private DoublyLinkedList tracks;
            private DNode currentTrack; // current playlist position
    
            public Playlist(string name)
            {
                this.name = name;
                count = 0;
                tracks = new DoublyLinkedList();
                currentTrack = null; // Initialize current track to null
            }
    
            public string GetName() { return name; }
            public int GetCount() { return count; }
            public void SetName(string name) { this.name = name; }
    
            public void Add(Track t)
                {
                    DNode newNode = new DNode(t, null, null);
                    tracks.AddLast(newNode);
                    count++;
                
                    // If this is the first track added, setting it as the current track
                    if (currentTrack == null)
                        currentTrack = newNode;
                }

    
            public void Remove(Track t)
            {
                DNode current = tracks.GetFirst();
                while (current != null)
                {
                    if (current.GetTrack() == t)
                    {
                        tracks.Remove(current);
                        count--;
                        break;
                    }
                    current = current.GetNext();
                }
            }
    
            public void Next()
            {
                if (currentTrack == null)
                {
                    Console.WriteLine("No track currently playing.");
                    return;
                }
            
                if (tracks.HasNext(currentTrack))
                {
                    currentTrack = tracks.GetNext(currentTrack);
                }
                else
                {
                    Console.WriteLine("No next song available.");
                }
            }
            
            public void Previous()
            {
                if (currentTrack == null)
                {
                    Console.WriteLine("No track currently playing.");
                    return;
                }
            
                if (tracks.HasPrev(currentTrack))
                {
                    currentTrack = tracks.GetPrev(currentTrack);
                }
                else
                {
                    Console.WriteLine("No previous song available.");
                }
            }
    
            public override string ToString()
            {
                if (currentTrack == null)
                    return "No track currently playing.";
                else
                    Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                    Console.Write("||");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("  <  ►  >  ");
                    Console.ResetColor();
                    Console.Write("||");
                    Console.ForegroundColor = ConsoleColor.Green;
                    return ("Currently playing: "+currentTrack.GetTrack().GetName()+" by "+currentTrack.GetTrack().GetArtistName()+" || Album : "+currentTrack.GetTrack().GetAlbumName()+" || Duration : "+currentTrack.GetTrack().GetDuration());
            }
            
            public void ab(DNode v, Track newTrack)
            {
                if (v == null || newTrack == null)
                {
                    Console.WriteLine("Invalid node or track. Track not added.");
                    return;
                }
            
                DNode newNode = new DNode(newTrack, null, null);
                tracks.AddBefore(v, newNode);
                count++;
            
                // If the new track is added before the current track, update currentTrack
                if (currentTrack == v)
                    currentTrack = newNode;
            }
            
            public void af(DNode v, Track newTrack)
            {
                if (v == null || newTrack == null)
                {
                    Console.WriteLine("Invalid node or track. Track not added.");
                    return;
                }
            
                DNode newNode = new DNode(newTrack, null, null);
                tracks.AddAfter(v, newNode);
                count++;
            
                // If the new track is added after the current track, update currentTrack
                if (currentTrack == v)
                    currentTrack = newNode;
            }
            
            public DNode GetCurrentTrackNode()
            {
                return currentTrack;
            }


    
            // Optional: Shuffle method
            public void Shuffle()
            {
                // Randomly rearrange the songs in the playlist
                List<DNode> shuffledList = new List<DNode>();
                DNode current = tracks.GetFirst();
                DNode lastNode = tracks.GetLast();
                
                while (current != lastNode)
                {
                    shuffledList.Add(current);
                    current = tracks.GetNext(current);
                }
            
                Random rand = new Random();
                for (int i = shuffledList.Count - 1; i > 0; i--)
                {
                    int j = rand.Next(0, i + 1);
                    DNode temp = shuffledList[i];
                    shuffledList[i] = shuffledList[j];
                    shuffledList[j] = temp;
                }
            
                // Clear the existing playlist except the tail node
                tracks = new DoublyLinkedList();
                foreach (DNode node in shuffledList)
                {
                    tracks.AddLast(node);
                }
                tracks.AddLast(lastNode); // Adding the tail node back
            }
            
            public void RemoveCurrentTrack()
            {
                if (currentTrack == null)
                {
                    Console.WriteLine("No track currently playing.");
                    return;
                }
            
                tracks.Remove(currentTrack);
                count--;
            
                // After removing the current track, set currentTrack to the next track if available,
                // otherwise, set it to the previous track if available.
                if (tracks.HasNext(currentTrack))
                    currentTrack = tracks.GetNext(currentTrack);
                else if (tracks.HasPrev(currentTrack))
                    currentTrack = tracks.GetPrev(currentTrack);
                else
                    currentTrack = null;
            
                Console.WriteLine("Current track removed.");
            }
        }

        static void Main(string[] args)
        {
            Playlist myPlaylist = new Playlist("My Playlist");

            // Add some initial tracks
            Track track1 = new Track("Closer", "Chainsmokers", "Summer Album", 180);
            Track track2 = new Track("New Divide", "Linkin Park", "Pop", 240);
            Track track3 = new Track("Lonely", "Akon", "Old School", 220);
            myPlaylist.Add(track1);
            myPlaylist.Add(track2);
            myPlaylist.Add(track3);
            int pos = 1;

            // Display current playlist details
            Console.WriteLine($"Playlist: {myPlaylist.GetName()}, Total Tracks: {myPlaylist.GetCount()}");

            // Display current song
            Console.Write(myPlaylist.ToString());
            Console.ResetColor();
            Console.WriteLine(" ||");
            Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");

            // User interaction: move to next or previous song
            bool exit = false;
            Console.WriteLine("Enter 'n' to play the next song, 'p' to play the previous song, 'a' to add a new track, 'd' to delete the current song, 'q' to quit:");
            while (!exit)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "n":
                        if(myPlaylist.GetCount() == pos)
                        {
                            Console.WriteLine("No next song available");
                            break;
                        }
                        else
                        {
                            myPlaylist.Next();
                            Console.Write(myPlaylist.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" ||");
                            Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                            pos++;
                            break;
                        }
                    case "p":
                        if(pos == 1)
                        {
                            Console.WriteLine("No previous song available");
                            break;
                        }
                        else
                        {
                            myPlaylist.Previous();
                            Console.Write(myPlaylist.ToString());
                            Console.ResetColor();
                            Console.WriteLine(" ||");
                            Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                            pos--;
                            break;
                        }
                    case "a":
                        // Gather track details from the user
                        Console.Write("Enter track name: ");
                        string trackName = Console.ReadLine();
                        Console.Write("Enter artist name: ");
                        string artistName = Console.ReadLine();
                        Console.Write("Enter album name: ");
                        string albumName = Console.ReadLine();
                        Console.Write("Enter track duration (in seconds): ");
                        int duration = int.Parse(Console.ReadLine());
                    
                        Track newTrack = new Track(trackName, artistName, albumName, duration);
                    
                        // Ask the user if they want to add the new track before or after an existing track
                        Console.WriteLine("Enter 'b' to add the new track before the current track, 'a' to add after:");
                        string positionInput = Console.ReadLine();
                        switch (positionInput)
                        {
                            case "b":
                                // Add the new track before the current track
                                myPlaylist.ab(myPlaylist.GetCurrentTrackNode(), newTrack);
                                pos--;
                                break;
                            case "a":
                                // Add the new track after the current track
                                myPlaylist.af(myPlaylist.GetCurrentTrackNode(), newTrack);
                                pos++;
                                break;
                            default:
                                Console.WriteLine("Invalid input. Track not added.");
                                break;
                        }
                    
                        // Display updated playlist details
                        Console.WriteLine($"Playlist: {myPlaylist.GetName()}, Total Tracks: {myPlaylist.GetCount()}");
                        Console.Write(myPlaylist.ToString());
                        Console.ResetColor();
                        Console.WriteLine(" ||");
                        Console.WriteLine("|||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
                        break;
                    case "d":
                        myPlaylist.RemoveCurrentTrack();
                        Console.Write(myPlaylist.ToString());
                        Console.ResetColor();
                        Console.WriteLine(" ||");
                        pos -- ;
                        break;
                        
                        //Commenting out the shuffle case. Shuffling works, but causes the current node pointer to go out of sync.
                        //Screenshots attached for shuffle functionality.
                    /*case "s":
                        myPlaylist.Shuffle();
                        Console.WriteLine("Playlist shuffled.");
                        break; */
                        
                    case "q":
                        exit = true;
                        Console.WriteLine("-------------------------------------------");
                        Console.WriteLine("Closing the playlist. Thanks for listening!");
                        Console.WriteLine("-------------------------------------------");
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }
    }
}




