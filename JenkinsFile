pipeline{
  agent any
  
  stages {
  
    stage ('Clean workspace') {
      steps {
        cleanWs()
      }
    }
    
    stage('Restore packages') {
      steps {
        bat "dotnet restore ${workspace}\\https://github.com/JoroPaspalev/joro-paspalev-employees\\Couple_Employees.sln"
      }
    }
  
    stage('Compiling the code here'){
      
      steps 
    }
  }
}
