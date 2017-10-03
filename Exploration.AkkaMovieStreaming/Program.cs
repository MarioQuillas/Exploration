namespace Exploration.AkkaMovieStreaming
{
    using System;

    using Akka.Actor;

    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            Console.ReadLine();

            MovieStreamingActorSystem.Terminate();
        }
    }
}
