using System;
using System.Collections.Generic;
using System.Text;

namespace NChordLib
{
    public static partial class ChordServer
    {
        /// <summary>
        /// Chama AddKey() remotamente, usando o valor de tentativas 3
        /// </summary>
        /// <param name="remoteNode">O nó remoto ao qual a chamada será feita</param>
        /// <param name="value">A string a ser adicionada.</param>
        public static void CallAddKey(ChordNode remoteNode, string value, string region)
        {
            CallAddKey(remoteNode, value, region, 3);
        
        }

        /// <summary>
        /// Chama AddKey() remotamente, com definição do numero de tentativas
        /// </summary>
        /// <param name="remoteNode">O nó remoto ao qual a chamada será feita</param>
        /// <param name="value">A string a ser adicionada.</param>
        /// <param name="retryCount">Número de tentativas</param>
        public static void CallAddKey(ChordNode remoteNode, string value, string region, int retryCount)
        {
            ChordInstance instance = ChordServer.GetInstance(remoteNode);

            try
            {
                instance.AddKey(value, region);
            }
            catch (System.Exception ex)
            {
                ChordServer.Log(LogLevel.Debug, "Remote Invoker", "CallAddKey error: {0}", ex.Message);
                if (retryCount>0)
                {
                    CallAddKey(remoteNode, value, region, --retryCount);
                }
                else
                {
                    ChordServer.Log(LogLevel.Debug, "Remote Invoker", "CallAddKey failed - error: {0}", ex.Message);
                }
            }
        
        }

        /// <summary>
        /// Chama FindKey() remotamente, usando o numero de tentativas default de 3.
        /// </summary>
        /// <param name="remoteNode">O nó remoto ao qual a chamada será feita</param>
        /// <param name="key">A string a ser encontrada.</param>
        /// <returns>Valor da chave ou uma string vazia se não encontrada.</returns>
        public static string CallFindKey(ChordNode remoteNode, ulong key)
        {
            return CallFindKey(remoteNode, key, 3);
        }

        /// <summary>
        /// Chama FindKey remotamente, definindo o número de tentativas.
        /// </summary>
        /// <param name="remoteNode">Onó remoto ao qual a chamada será feita.</param>
        /// <param name="key">A string a ser encontrada.</param>
        /// <param name="retryCount">Número de tentativas.</param>
        /// <returns> O valor correspondendo a chave, ou vazio se ela não existir.</returns>
        public static string CallFindKey(ChordNode remoteNode, ulong key, int retryCount)
        {
            ChordInstance instance = ChordServer.GetInstance(remoteNode);
            try
            {
                return instance.FindKey(key);
            }
            catch (System.Exception ex)
            {
                ChordServer.Log(LogLevel.Debug, "Remote Invoker", "CallFindKey error: {0}", ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Chama ReplicateKey() remotamente, usando parâmetro de 3 tentativas.
        /// </summary>
        /// <param name="remoteNode">Nó remoto a ser chamado o método.</param>
        /// <param name="key">A chave a ser replicada.</param>
        /// <param name="value">Valor da string a ser replicada.</param>
        public static void CallReplicateKey(ChordNode remoteNode, ulong key, string value, string region)
        {
            CallReplicateKey(remoteNode, key, value, region, 3);
        }

        /// <summary>
        /// Chama ReplicateKey() remotamente, usando parâmetro como argumento.
        /// </summary>
        /// <param name="remoteNode">Nó remoto a ser chamado o método.</param>
        /// <param name="key">A chave a ser replicada.</param>
        /// <param name="value">Valor da string a ser replicada.</param>
        /// <param name="retryCount">número de tentativas.</param>
        public static void CallReplicateKey(ChordNode remoteNode, ulong key, string value, string region,  int retryCount)
        {
            ChordInstance instance = ChordServer.GetInstance(remoteNode);
            try
            {
                instance.ReplicateKey(key, value, region);
            }
            catch (System.Exception ex)
            {
                ChordServer.Log(LogLevel.Debug, "Remote Invoker", "CallRelicateKey error: {0}", ex.Message);
                if (retryCount > 0)
                {
                    CallReplicateKey(remoteNode, key, value, region, --retryCount);
                }
                else
                {
                    ChordServer.Log(LogLevel.Debug, "RemoteInvoker", "CallReplicateKey failed - error:{0}", ex.Message);
                }
            }

        }

    }
}
