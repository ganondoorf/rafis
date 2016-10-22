/*
 * ChordInstance.Storage.cs;
 * 
 * Contém a estrutura de dados privados e métodos públicos para armazenamento dos pares chave-valor.
 *
 */


using System;
using System.Collections.Generic;
using System.Text;

namespace NChordLib
{
    public partial class ChordInstance : MarshalByRefObject
    {
        /// <summary>
        /// Armazenada a string de dados como um valor de chave de 64 bits.
        /// </summary>
        private SortedList<ulong, string> m_DataStore = new SortedList<ulong, string>();
        private SortedList<ulong, string> m_DataRegion = new SortedList<ulong, string>();

        public SortedList<ulong, string> PrintKeys()
        {
            return m_DataStore;
        }

        public string PrintRegion(ulong key)
        {
            return m_DataRegion[key];
        }

        
        /// <summary>
        /// Adiciona a chave localmente. Utilizando o hash da chave determina o nó proprietário correto e armazena o valor da string neste nó.
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void AddLocalKey(string value, string region)
        //public void AddKey(ulong key)
        {

            //A chave é o hash do valor a ser adicionado no registro e determina o nó Nchord proprietário.
            //ulong key = UInt64.Parse(value);
            ulong key = ChordServer.GetHash(value);
            this.m_DataStore.Add(key, value);
            this.m_DataRegion.Add(key, region);
            
        }
        /// <summary>
        /// Adiciona a chave. Utilizando o hash da chave determina o nó proprietário correto e armazena o valor da string neste nó.
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void AddKey(string value, string region)
        //public void AddKey(ulong key)
        {

            //A chave é o hash do valor a ser adicionado no registro e determina o nó Nchord proprietário.
            //ulong key = UInt64.Parse(value);
            ulong key = ChordServer.GetHash(value);

            // usando o nó local determina o nó dono correto para os dados serem armazenados, dado o valor da chave.
            // Diferente da documentação
            ChordNode owningNode = ChordServer.DCallFindSuccessor(key); 

            if (owningNode.ToString() != ChordServer.LocalNode.ToString())
            {
                //Se este não é o nó proprietário, chame AddKey no nó atual.
                //ChordInstance remoteInstance = ChordServer.GetInstance(owningNode);
                //remoteInstance.AddKey(value);
                ChordServer.CallAddKey(owningNode, value, region);
            }
            else
            {
                //Se este é o nó proprietário, então adicione a chave na base local.
                if (m_DataStore.ContainsKey(key))
                {
                    //Console.WriteLine("Chave já utilizada.");
                }
                else
                {
                    try
                    {
                        this.m_DataStore.Add(key, value);
                        this.m_DataRegion.Add(key, region);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Não foi possível adicionar a chave na base: "+e);

                    }
                }

                
                
            }
        }

        /// <summary>
        /// Adição não original. Adiciona a chave e o valor do ID. Utilizando o hash da chave determina o nó proprietário correto e armazena o valor da string neste nó.
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void AddKey(ulong key, string value, string region)
        //public void AddKey(ulong key)
        {

            //A chave é o hash do valor a ser adicionado no registro e determina o nó Nchord proprietário.
            //ulong key = UInt64.Parse(value);
            //ulong key = ChordServer.GetHash(value);

            // usando o nó local determina o nó dono correto para os dados serem armazenados, dado o valor da chave.
            // Diferente da documentação
            ChordNode owningNode = ChordServer.DCallFindSuccessor(key);

            if (owningNode.ToString() != ChordServer.LocalNode.ToString())
            {
                //Se este não é o nó proprietário, chame AddKey no nó atual.
                //ChordInstance remoteInstance = ChordServer.GetInstance(owningNode);
                //remoteInstance.AddKey(value);
                ChordServer.CallAddKey(owningNode, value, region);
            }
            else
            {
                //Se este é o nó proprietário, então adicione a chave na base local.
                this.m_DataStore.Add(key, value);
                this.m_DataRegion.Add(key, region);
            }
        }




        /// <summary>
        /// Retorna o valor da string para uma kave ulong. 
        /// </summary>
        /// <param name="key">A chave cujo valor deve ser retornado.</param>
        /// <returns>O valor da string para a chave dada, ou empty se não encontrada.</returns>
        public string FindKey(ulong key)
        {
            // detreminado o nõ proprietário, dado a chave.
            ChordNode owningNode = ChordServer.DCallFindSuccessor(key);
            string test1, test2;
            test1=owningNode.ToString();
            test2=ChordServer.LocalNode.ToString();

            if (owningNode.ToString() != ChordServer.LocalNode.ToString())
            {
                // se este não é o nó proprietário, chame FindKey no nó proprietário remoto
                //ChordInstance remoteInstance = ChordServer.GetInstance(owningNode);
                //return remoteInstance.FindKey(key);
                return ChordServer.CallFindKey(owningNode, key);
            }
            else
            {
                // se este é o nó proprietário, cheque se o mesmo exite no registro. 
                if (this.m_DataStore.ContainsKey(key))
                {
                    // se existe, retorne o valor. 
                    return this.m_DataStore[key]+":"+string.Format("0x{0:X}", key)+" -> "+this.m_DataRegion[key]+": "+this.Port;
                }
                else
                {
                    // se não existe, retorne empty, string vazia. 
                    return string.Empty;
                }
            }

        }

        /// <summary>
        /// Retorna o Host para uma chave ulong. 
        /// </summary>
        /// <param name="key">A chave cujo valor deve ser retornado.</param>
        /// <returns>O host para a chave dada, ou empty se não encontrada.</returns>
        public string FindHost(ulong key)
        {
            // detreminado o nõ proprietário, dado a chave.
            ChordNode owningNode = ChordServer.DCallFindSuccessor(key);
            
            if (owningNode.ToString() != ChordServer.LocalNode.ToString())
            {
                // se este não é o nó proprietário, chame FindKey no nó proprietário remoto
                //ChordInstance remoteInstance = ChordServer.GetInstance(owningNode);
                //return remoteInstance.FindKey(key);
                //return ChordServer.CallFindKey(owningNode, key);
                return owningNode.Host + ":" + owningNode.PortNumber;
            }
            else
            {
                // se este é o nó proprietário, cheque se o mesmo exite no registro. 
                if (this.m_DataStore.ContainsKey(key))
                {
                    // se existe, retorne o valor. 
                    //return this.m_DataRegion[key]+owningNode.PortNumber;
                    return ChordServer.LocalNode.Host+":"+ChordServer.LocalNode.PortNumber;
                }
                else
                {
                    // se não existe, retorne empty, string vazia. 
                    return string.Empty;
                }
            }

        }


        /// <summary>
        /// Sincroniza a chave. Utilizando o hash da chave determina o nó proprietário correto e armazena o valor da string neste nó.
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void syncKey(string value, string region)
        //public void AddKey(ulong key)
        {

            //A chave é o hash do valor a ser adicionado no registro e determina o nó Nchord proprietário.
            //ulong key = UInt64.Parse(value);
            ulong key = ChordServer.GetHash(value);

            // usando o nó local determina o nó dono correto para os dados serem armazenados, dado o valor da chave.
            // Diferente da documentação
            ChordNode owningNode = ChordServer.DCallFindSuccessor(key);

            if (owningNode.ToString() != ChordServer.LocalNode.ToString())
            {
                //Se este não é o nó proprietário, chame AddKey no nó atual.
                //ChordInstance remoteInstance = ChordServer.GetInstance(owningNode);
                //remoteInstance.AddKey(value);
                ChordServer.CallAddKey(owningNode, value, region);
            }
        }


        /// <summary>
        /// Adiciona uma réplica par valor/chave ao registro local.
        /// </summary>
        /// <param name="key">chave a ser replicada</param>
        /// <param name="value">valor a ser replicado</param>
        public void ReplicateKey(ulong key, string value)
        { 
            //adiciona a chave/valor ao registro local, independente do proprietário.
            if (!this.m_DataStore.ContainsKey(key))
            {
                this.m_DataStore.Add(key, value);
            }
        }

        /// <summary>
        /// Adicionado ao original. Adiciona uma réplica par valor/chave ao registro local.
        /// </summary>
        /// <param name="key">chave a ser replicada</param>
        /// <param name="value">valor a ser replicado</param>
        public void ReplicateKey(ulong key, string value, string region)
        {
            //adiciona a chave/valor ao registro local, independente do proprietário.
            if (!this.m_DataStore.ContainsKey(key))
            {
                this.m_DataStore.Add(key, value);
                this.m_DataRegion.Add(key, region);
            }
        }

    }

}
    



