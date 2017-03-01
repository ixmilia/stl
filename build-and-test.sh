#!/bin/sh

TEST_PROJECT=./src/IxMilia.Stl.Test/IxMilia.Stl.Test.csproj
dotnet restore $TEST_PROJECT
dotnet test $TEST_PROJECT
