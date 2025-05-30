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

using System;
using System.Threading.Tasks;

namespace Apache.Arrow.Adbc.Drivers.BigQuery
{
    /// <summary>
    /// Common interface for a token protected resource.
    /// </summary>
    internal interface ITokenProtectedResource
    {
        /// <summary>
        /// The function to call when updating the token.
        /// </summary>
        Func<Task>? UpdateToken { get; set; }

        /// <summary>
        /// Determines the token needs to be updated.
        /// </summary>
        /// <param name="ex">The exception that occurs.</param>
        /// <returns>True/False indicating a refresh is needed.</returns>
        bool TokenRequiresUpdate(Exception ex);
    }
}
