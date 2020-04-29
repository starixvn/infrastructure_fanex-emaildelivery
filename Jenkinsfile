//Test
@Library("JenkinsPipelineLibrary") _

pipeline {  
  agent {
    label "slaveSUN"
  }

  options{
    buildDiscarder(logRotator(numToKeepStr: '5'))    
    disableConcurrentBuilds()
    gitLabConnection('Gitlab')
  }
  triggers{
    gitlab(triggerOnPush: true, triggerOnMergeRequest: true, branchFilterType: 'All')
  } 
  stages {
    stage('Build') {
      steps {       
        script{
          pipelineLib.beginSonarQubeForMsBuild("MailStation", "NexOps / Mail Station", "")

          pipelineLib.restoreNuget()

          pipelineLib.msBuild("Fanex.MailDelivery.sln")
        }
      }    
    }

    stage("C# Test"){
      steps{
        script{
          pipelineLib.xUnitForNetFramework()
        }
      }
    }

    stage("SonarQube Analysis"){
      steps{       
        script{
          pipelineLib.endSonarQubeForMsBuild()
        }
      }
      
      post{
        always{
          script{
            if(manager.logContains(".*Quality gate status.*")){              
              pipelineLib.generateSonarQubeReport("MailStation")
            }
          }
        }
      }
    }
  }

  post{
    always{
      archiveArtifacts "*.xml,*.email"
      step([$class: 'ACIPluginPublisher', name: '*.xml', shownOnProjectPage: false])
    }
    unsuccessful{
      emailext body: '$DEFAULT_CONTENT', subject: '$DEFAULT_SUBJECT', to: 'larry.tran@starixsoft.com'
    }
  }
}