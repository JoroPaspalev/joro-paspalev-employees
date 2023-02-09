pipeline{
  agent any
  
  stages {
  
    stage ('Clean workspace') {
      steps {
        cleanWs()
      }
    }
    
    stage ('Git Checkout') {
      steps {
         git branch: 'master', url: 'https://github.com/JoroPaspalev/joro-paspalev-employees.git'
      }
    }
    
    stage('Restore packages') {
      steps {
        bat "dotnet restore ${workspace}\\Couple_Employees.sln"
      }
    }
    
    stage('Build') {
       steps {
        bat "dotnet build ${workspace}\\Couple_Employees.sln /nologo /nr:false  /p:platform=\"x64\" /p:configuration=\"release\" /t:clean;restore;rebuild"
       }
    }
  
    
  }
}
