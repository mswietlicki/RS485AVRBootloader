﻿using System.IO;
using System.Linq;
using SerialAVRBootloader.Loader.Common;
using SerialAVRBootloader.Loader.Communicators;
using SerialAVRBootloader.Loader.Exceptions;
using SerialAVRBootloader.Loader.Model;
using System.Threading;

namespace SerialAVRBootloader.Loader
{
    public class BootloaderCommunicator
    {
        private readonly ICommunicator _communicator;
        private readonly ILogger _logger;

        public BootloaderCommunicator(ICommunicator communicator, ILogger logger)
        {
            _communicator = communicator;
            _logger = logger;
        }


        public BootloaderInfo GetBootloaderInfo()
        {
            if (_communicator.ReadChar() != '?')
                throw new CommunicationLostExpection();

            _communicator.Write("ui");

            var rawInfo = _communicator.ReadTo("?");

            return BootloaderInfo.Parse(rawInfo);
        }

        public void WriteProgram(Stream dataStream)
        {
            if (_communicator.ReadChar() != '?')
                throw new CommunicationLostExpection();

            _communicator.Write("uw");
            _communicator.Write(new byte[] { 1 }, 0, 1);
            bool endofFile = false;
            while (!endofFile && dataStream.Position < dataStream.Length)
            {
                 //Send start 1
                var data = new byte[65];
                data[64] = 1;

                if (dataStream.Position + 64 >= dataStream.Length)
                {
                    var toEnd = (int)(dataStream.Length - dataStream.Position);
                    dataStream.Read(data, 0, toEnd);

                    if (dataStream.CanSeek)
                    {
                        dataStream.Seek(0, SeekOrigin.Begin);
                        dataStream.Read(data, toEnd, 64 - toEnd);
                    }
                    data[64] = 0;
                    endofFile = true;
                }
                else
                {
                    dataStream.Read(data, 0, 64);
                }


                _communicator.Write(data, 0, 65);


               var ack = _communicator.ReadChar();
                    
                    //if (new[] { '@', (char)0xA0 }.Contains(ack) == false)
                    //    throw new CommunicationLostExpection();
                //}
            }
            _communicator.Write(new byte[1], 0, 1); //Send end 0

            //var gfh = _communicator.ReadChar();
        }
    }
}