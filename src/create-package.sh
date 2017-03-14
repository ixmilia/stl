#!/bin/sh -e

PROJECT=./IxMilia.Stl/IxMilia.Stl.csproj
dotnet restore $PROJECT
dotnet pack --include-symbols --include-source --configuration Release $PROJECT

