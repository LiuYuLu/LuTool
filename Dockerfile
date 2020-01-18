FROM home.istr.cn:58082/core/aspnet:3.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 48919
ENV TZ=Asia/Shanghai

#FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
#WORKDIR /
#COPY . .
#RUN dotnet publish -c:Release -o ./publish

#FROM base AS final
#WORKDIR /Istr
COPY /publish .
RUN ls -la
ENTRYPOINT ["dotnet", "Istr.dll"]