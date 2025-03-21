﻿/*
* Licensed to the Apache Software Foundation (ASF) under one or more
* contributor license agreements.  See the NOTICE file distributed with
* this work for additional information regarding copyright ownership.
* The ASF licenses this file to You under the Apache License, Version 2.0
* (the "License"); you may not use this file except in compliance with
* the License.  You may obtain a copy of the License at
*
*    http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System.Data.SqlTypes;
using System.Threading.Tasks;
using Apache.Arrow.Adbc.Drivers.Apache.Hive2;
using Xunit;
using Xunit.Abstractions;

namespace Apache.Arrow.Adbc.Tests.Drivers.Apache.Common
{
    // TODO: When supported, use prepared statements instead of SQL string literals
    //      Which will better test how the driver handles values sent/received

    public abstract class NumericValueTests<TConfig, TEnv> : TestBase<TConfig, TEnv>
        where TConfig : TestConfiguration
        where TEnv : CommonTestEnvironment<TConfig>
    {
        /// <summary>
        /// Validates that specific numeric values can be inserted, retrieved and targeted correctly
        /// </summary>
        public NumericValueTests(ITestOutputHelper output, TestEnvironment<TConfig>.Factory<TEnv> testEnvFactory)
            : base(output, testEnvFactory) { }

        /// <summary>
        /// Validates if driver can send and receive specific Integer values correctly
        /// </summary>
        [SkippableTheory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public async Task TestIntegerSanity(int value)
        {
            string columnName = "INTTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} INT", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, value);
        }

        /// <summary>
        /// Validates if driver can handle the largest / smallest numbers
        /// </summary>
        [SkippableTheory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task TestIntegerMinMax(int value)
        {
            string columnName = "INTTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} INT", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, value);
        }

        /// <summary>
        /// Validates if driver can handle the largest / smallest numbers
        /// </summary>
        [SkippableTheory]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        public async Task TestLongMinMax(long value)
        {
            string columnName = "BIGINTTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} BIGINT", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, value);
        }

        /// <summary>
        /// Validates if driver can handle the largest / smallest numbers
        /// </summary>
        [SkippableTheory]
        [InlineData(short.MaxValue)]
        [InlineData(short.MinValue)]
        public async Task TestSmallIntMinMax(short value)
        {
            string columnName = "SMALLINTTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} SMALLINT", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, value);
        }

        /// <summary>
        /// Validates if driver can handle the largest / smallest numbers
        /// </summary>
        [SkippableTheory]
        [InlineData(sbyte.MaxValue)]
        [InlineData(sbyte.MinValue)]
        public async Task TestTinyIntMinMax(sbyte value)
        {
            string columnName = "TINYINTTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} TINYINT", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, value);
        }

        /// <summary>
        /// Validates if driver can handle smaller Number type correctly
        /// </summary>
        [SkippableTheory]
        [InlineData("-1")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("99")]
        [InlineData("-99")]
        public async Task TestSmallNumberRange(string value)
        {
            string columnName = "SMALLNUMBER";
            string columnType = "DECIMAL(2,0)";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} {1}", columnName, columnType));
            object? expectedValue = TestEnvironment.GetValueForProtocolVersion(value, SqlDecimal.Parse(value));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, expectedValue, $"CAST({expectedValue} as {columnType})");
        }

        ///// <summary>
        ///// Validates if driver correctly errors out when the values exceed the column's limit
        ///// </summary>
        //[SkippableTheory]
        //[InlineData(-100)]
        //[InlineData(100)]
        //[InlineData(int.MaxValue)]
        //[InlineData(int.MinValue)]
        //public async Task TestSmallNumberRangeOverlimit(int value)
        //{
        //    string columnName = "SMALLNUMBER";
        //    using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DECIMAL(2,0)", columnName));
        //    await Assert.ThrowsAsync<HiveServer2Exception>(
        //        async () => await ValidateInsertSelectDeleteSingleValueAsync(
        //            table.TableName,
        //            columnName, TestEnvironment.GetValueForProtocolVersion(value.ToString(), new SqlDecimal(value))));
        //}

        /// <summary>
        /// Validates if driver can handle a large scale Number type correctly
        /// </summary>
        [SkippableTheory]
        [InlineData("0E-37")]
        [InlineData("-2.0030000000000000000000000000000000000")]
        [InlineData("4.8500000000000000000000000000000000000")]
        [InlineData("1E-37")]
        [InlineData("9.5545204502636499875576383003668916798")]
        public async Task TestLargeScaleNumberRange(string value)
        {
            string columnName = "LARGESCALENUMBER";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DECIMAL(38,37)", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, TestEnvironment.GetValueForProtocolVersion(value, new SqlDecimal(double.Parse(value))));
        }

        ///// <summary>
        ///// Validates if driver can error handle when input goes beyond a large scale Number type
        ///// </summary>
        //[SkippableTheory]
        //[InlineData("-10")]
        //[InlineData("10")]
        //[InlineData("99999999999999999999999999999999999999")]
        //[InlineData("-99999999999999999999999999999999999999")]
        //public async Task TestLargeScaleNumberOverlimit(string value)
        //{
        //    string columnName = "LARGESCALENUMBER";
        //    using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DECIMAL(1,0)", columnName));
        //    await Assert.ThrowsAsync<HiveServer2Exception>(async () => await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, SqlDecimal.Parse(value)));
        //}

        /// <summary>
        /// Validates if driver can handle a small scale Number type correctly
        /// </summary>
        [SkippableTheory]
        [InlineData("0.00")]
        [InlineData("4.85")]
        [InlineData("-999999999999999999999999999999999999.99")]
        [InlineData("999999999999999999999999999999999999.99")]
        public async Task TestSmallScaleNumberRange(string value)
        {
            string columnName = "SMALLSCALENUMBER";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DECIMAL(38,2)", columnName));
            await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, TestEnvironment.GetValueForProtocolVersion(value, SqlDecimal.Parse(value)));
        }

        ///// <summary>
        ///// Validates if driver can error handle when an insert goes beyond a small scale Number type correctly
        ///// </summary>
        //[SkippableTheory]
        //[InlineData("-99999999999999999999999999999999999999")]
        //[InlineData("99999999999999999999999999999999999999")]
        //public async Task TestSmallScaleNumberOverlimit(string value)
        //{
        //    string columnName = "SMALLSCALENUMBER";
        //    using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DECIMAL(38,2)", columnName));
        //    await Assert.ThrowsAsync<HiveServer2Exception>(async () => await ValidateInsertSelectDeleteSingleValueAsync(table.TableName, columnName, SqlDecimal.Parse(value)));
        //}

        /// <summary>
        /// Tests that decimals are rounded as expected.
        /// Snowflake allows inserts of scales beyond the data type size, but storage of value will round it up or down
        /// </summary>
        [SkippableTheory]
        [InlineData(2.467, 2.47)]
        [InlineData(-672.613, -672.61)]
        public async Task TestRoundingNumbers(decimal input, decimal output)
        {
            string columnName = "SMALLSCALENUMBER";
            string columnType = "DECIMAL(38,2)";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} {1}", columnName, columnType));
            SqlDecimal value = new SqlDecimal(input);
            SqlDecimal returned = new SqlDecimal(output);
            await InsertSingleValueAsync(table.TableName, columnName, $"CAST({value} as {columnType})");
            await SelectAndValidateValuesAsync(table.TableName, columnName, TestEnvironment.GetValueForProtocolVersion(output.ToString(), returned), 1);
            string whereClause = GetWhereClause(columnName, returned);
            if (SupportsDelete) await DeleteFromTableAsync(table.TableName, whereClause, 1);
        }

        /// <summary>
        /// Validates if driver can handle floating point number type correctly
        /// </summary>
        public virtual async Task TestDoubleValuesInsertSelectDelete(double value)
        {
            string columnName = "DOUBLETYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} DOUBLE", columnName));
            string valueString = ConvertDoubleToString(value);
            await InsertSingleValueAsync(table.TableName, columnName, valueString);
            await SelectAndValidateValuesAsync(table.TableName, columnName, value, 1);
            string whereClause = GetWhereClause(columnName, value);
            if (SupportsDelete) await DeleteFromTableAsync(table.TableName, whereClause, 1);
        }

        /// <summary>
        /// Validates if driver can handle floating point number type correctly
        /// </summary>
        public virtual async Task TestFloatValuesInsertSelectDelete(float value)
        {
            string columnName = "FLOATTYPE";
            using TemporaryTable table = await NewTemporaryTableAsync(Statement, string.Format("{0} FLOAT", columnName));
            string valueString = ConvertFloatToString(value);
            await InsertSingleValueAsync(table.TableName, columnName, valueString);
            object doubleValue = (double)value;
            // Spark over HTTP returns float as double whereas Spark on Databricks returns float.
            object floatValue = TestEnvironment.DataTypeConversion.HasFlag(DataTypeConversion.Scalar) ? value : doubleValue;
            await SelectAndValidateValuesAsync(table.TableName, columnName, floatValue, 1);
            string whereClause = GetWhereClause(columnName, value);
            if (SupportsDelete) await DeleteFromTableAsync(table.TableName, whereClause, 1);
        }
    }
}
