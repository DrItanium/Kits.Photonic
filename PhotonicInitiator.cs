//Copyright 2012 Joshua Scoggins. All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are
//permitted provided that the following conditions are met:
//
//   1. Redistributions of source code must retain the above copyright notice, this list of
//      conditions and the following disclaimer.
//
//   2. Redistributions in binary form must reproduce the above copyright notice, this list
//      of conditions and the following disclaimer in the documentation and/or other materials
//      provided with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY Joshua Scoggins ``AS IS'' AND ANY EXPRESS OR IMPLIED
//WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL Joshua Scoggins OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
//SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
//ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
//ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
//The views and conclusions contained in the software and documentation are those of the
//authors and should not be interpreted as representing official policies, either expressed
//or implied, of Joshua Scoggins. 
//
//
//The Idea behind this object is that we need to add a layer of indirection
//between the application domains to prevent other domains from loading
//assemblies from another.
//
//
using System;
using System.Runtime;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace Kits.Photonic
{
  public class IPhotonicInitiator
  {
    Guid Unique ID { get; }
    bool TypeNameDefined(string name);
    bool AssemblyLoaded(string name);
    CommunicationPrimitive Communicate(CommunicationPrimitive packet);
  }
  public class PhotonicInitiator : IPhotonicInitiator, MarshalByRefObject
  {
    private IPhotonicInitiator other;
    private Guid uniqueID;
    public Guid UniqueID { get { return uniqueID; } }
    public PhotonicInitiator AppDomainNode { get { return other; } }
    public PhotonicInitiator(IPhotonicInitiator other, Guid guid, 
        string intraDomainDispatcherAssemblyPath,
        string intraDomainDispatcherType)
    {
      uniqueID = guid;
      //TODO: Provide functionality to load the assembly and type in question
      //into memory. Once that is done it's neccessary to check if the
      //initiator implements a specific interface or extends off of a
      //predefined abstract class. If that yields success then it's necessary
      //to keep a copy of it and indirectly pass data to that object within
    } 
    public bool TypeNameDefined(string name)
    {
       AppDomain tmp = AppDomain.CurrentDomain;
       foreach(var asm in tmp.GetAssemblies())
       {
         foreach(var type in asm.GetTypes())
         {
           if(type.Equals(name))
             return true;
         }
       }
       return false;
    }
    public bool AssemblyLoaded(string name)
    {
      AppDomain tmp = AppDomain.CurrentDomain;
      foreach(var asm in tmp.GetAssemblies())
      {
        if(asm.FullName.Equals(name))
          return true;
      }
      return false;
    }
    public CommunicationPrimitive Communicate(CommunicationPrimitive cp)
    {
      //before we actually unwrap the contents lets do a test to make sure
      //we have the type in this app domain. We need to store the typename
      //within a separate string when creating the communication primitive
      if(!TypeNameDefined(cp.ContentType))
      {
        throw new ArgumentException("Provided type is not in the current application domain");
      }
      else
      {
        //TODO: Put body here where we pass the primitive off to the intra
        //appdomain communicator.
        return cp;
      }

    }
    //eventually we are going to need a command protocol to allow 
    //for cross application domain communication entirely through
    //strings or streams.
  }
}
