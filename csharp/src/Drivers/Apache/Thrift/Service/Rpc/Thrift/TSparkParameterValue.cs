/**
 * <auto-generated>
 * Autogenerated by Thrift Compiler (0.21.0)
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 * </auto-generated>
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Protocol.Entities;
using Thrift.Protocol.Utilities;
using Thrift.Transport;
using Thrift.Transport.Client;
using Thrift.Transport.Server;
using Thrift.Processor;


// targeting netstandard 2.x
#if(! NETSTANDARD2_0_OR_GREATER && ! NET6_0_OR_GREATER && ! NET472_OR_GREATER)
#error Unexpected target platform. See 'thrift --help' for details.
#endif

#pragma warning disable IDE0079  // remove unnecessary pragmas
#pragma warning disable IDE0017  // object init can be simplified
#pragma warning disable IDE0028  // collection init can be simplified
#pragma warning disable IDE1006  // parts of the code use IDL spelling
#pragma warning disable CA1822   // empty DeepCopy() methods still non-static
#pragma warning disable CS0618   // silence our own deprecation warnings
#pragma warning disable IDE0083  // pattern matching "that is not SomeType" requires net5.0 but we still support earlier versions

namespace Apache.Hive.Service.Rpc.Thrift
{

  internal partial class TSparkParameterValue : TBase
  {
    private string _stringValue;
    private double _doubleValue;
    private bool _booleanValue;

    public string StringValue
    {
      get
      {
        return _stringValue;
      }
      set
      {
        __isset.stringValue = true;
        this._stringValue = value;
      }
    }

    public double DoubleValue
    {
      get
      {
        return _doubleValue;
      }
      set
      {
        __isset.doubleValue = true;
        this._doubleValue = value;
      }
    }

    public bool BooleanValue
    {
      get
      {
        return _booleanValue;
      }
      set
      {
        __isset.booleanValue = true;
        this._booleanValue = value;
      }
    }


    public Isset __isset;
    public struct Isset
    {
      public bool stringValue;
      public bool doubleValue;
      public bool booleanValue;
    }

    public TSparkParameterValue()
    {
    }

    public async global::System.Threading.Tasks.Task ReadAsync(TProtocol iprot, CancellationToken cancellationToken)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        await iprot.ReadStructBeginAsync(cancellationToken);
        while (true)
        {
          field = await iprot.ReadFieldBeginAsync(cancellationToken);
          if (field.Type == TType.Stop)
          {
            break;
          }

          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String)
              {
                StringValue = await iprot.ReadStringAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 2:
              if (field.Type == TType.Double)
              {
                DoubleValue = await iprot.ReadDoubleAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            case 3:
              if (field.Type == TType.Bool)
              {
                BooleanValue = await iprot.ReadBoolAsync(cancellationToken);
              }
              else
              {
                await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              }
              break;
            default:
              await TProtocolUtil.SkipAsync(iprot, field.Type, cancellationToken);
              break;
          }

          await iprot.ReadFieldEndAsync(cancellationToken);
        }

        await iprot.ReadStructEndAsync(cancellationToken);
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public async global::System.Threading.Tasks.Task WriteAsync(TProtocol oprot, CancellationToken cancellationToken)
    {
      oprot.IncrementRecursionDepth();
      try
      {
        var tmp405 = new TStruct("TSparkParameterValue");
        await oprot.WriteStructBeginAsync(tmp405, cancellationToken);
        var tmp406 = new TField();
        if((StringValue != null) && __isset.stringValue)
        {
          tmp406.Name = "stringValue";
          tmp406.Type = TType.String;
          tmp406.ID = 1;
          await oprot.WriteFieldBeginAsync(tmp406, cancellationToken);
          await oprot.WriteStringAsync(StringValue, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.doubleValue)
        {
          tmp406.Name = "doubleValue";
          tmp406.Type = TType.Double;
          tmp406.ID = 2;
          await oprot.WriteFieldBeginAsync(tmp406, cancellationToken);
          await oprot.WriteDoubleAsync(DoubleValue, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        if(__isset.booleanValue)
        {
          tmp406.Name = "booleanValue";
          tmp406.Type = TType.Bool;
          tmp406.ID = 3;
          await oprot.WriteFieldBeginAsync(tmp406, cancellationToken);
          await oprot.WriteBoolAsync(BooleanValue, cancellationToken);
          await oprot.WriteFieldEndAsync(cancellationToken);
        }
        await oprot.WriteFieldStopAsync(cancellationToken);
        await oprot.WriteStructEndAsync(cancellationToken);
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override bool Equals(object that)
    {
      if (!(that is TSparkParameterValue other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return ((__isset.stringValue == other.__isset.stringValue) && ((!__isset.stringValue) || (global::System.Object.Equals(StringValue, other.StringValue))))
        && ((__isset.doubleValue == other.__isset.doubleValue) && ((!__isset.doubleValue) || (global::System.Object.Equals(DoubleValue, other.DoubleValue))))
        && ((__isset.booleanValue == other.__isset.booleanValue) && ((!__isset.booleanValue) || (global::System.Object.Equals(BooleanValue, other.BooleanValue))));
    }

    public override int GetHashCode() {
      int hashcode = 157;
      unchecked {
        if((StringValue != null) && __isset.stringValue)
        {
          hashcode = (hashcode * 397) + StringValue.GetHashCode();
        }
        if(__isset.doubleValue)
        {
          hashcode = (hashcode * 397) + DoubleValue.GetHashCode();
        }
        if(__isset.booleanValue)
        {
          hashcode = (hashcode * 397) + BooleanValue.GetHashCode();
        }
      }
      return hashcode;
    }

    public override string ToString()
    {
      var tmp407 = new StringBuilder("TSparkParameterValue(");
      int tmp408 = 0;
      if((StringValue != null) && __isset.stringValue)
      {
        if(0 < tmp408++) { tmp407.Append(", "); }
        tmp407.Append("StringValue: ");
        StringValue.ToString(tmp407);
      }
      if(__isset.doubleValue)
      {
        if(0 < tmp408++) { tmp407.Append(", "); }
        tmp407.Append("DoubleValue: ");
        DoubleValue.ToString(tmp407);
      }
      if(__isset.booleanValue)
      {
        if(0 < tmp408++) { tmp407.Append(", "); }
        tmp407.Append("BooleanValue: ");
        BooleanValue.ToString(tmp407);
      }
      tmp407.Append(')');
      return tmp407.ToString();
    }
  }

}
