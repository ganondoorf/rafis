/*
 * ChordInstance.Maintenance.ReplicateStorage.cs
 * 
 * Executa uma versão simples de replicação dos dados de registro
 * para o nó sucessor, como uma tarefa de manutenção.
 * 
 */

using System;
using System.ComponentModel;
using System.Threading;

namespace NChordLib
{
    public partial class ChordInstance : MarshalByRefObject
    {

        private void ReplicateStorage(object sender, DoWorkEventArgs ea)
        {
            BackgroundWorker me = (BackgroundWorker)sender;

            while (!me.CancellationPending)
            {
                try
                {
                    //replica cada chave para o sucessor de forma segura
                    foreach (ulong key in this.m_DataStore.Keys)
                    {
                        //se a chave é local (não copia réplicas)
                        if (ChordServer.IsIDInRange(key,this.ID,this.Successor.ID))
                        {
                            ChordServer.CallReplicateKey(this.Successor, key, this.m_DataStore[key], this.m_DataRegion[key]);
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    ChordServer.Log(LogLevel.Error, "Maintenance", "Error occured during ReplicateStorage ({0})", e.Message);
                }
                
                // TODO: fazer este valor configurável via config file ou passado como argumento.
                Thread.Sleep(30000);
            }
        
        }
    }
}
