using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Internal
{
    internal class ObjectStateManager
    {
        private HashSet<WsObject> _addedContainer = new HashSet<WsObject>();
        private HashSet<WsObject> _modifiedContainer = new HashSet<WsObject>();
        private HashSet<WsObject> _deletedContainer = new HashSet<WsObject>();

        private Type itemsType = typeof(object);

        public ObjectStateManager(Type baseType)
        {
            itemsType = baseType;
        }

        public Type BaseItemType
        {
            get { return itemsType; }
        }

        public bool AddObject(WsObject objectToAdd)
        {
            return _addedContainer.Add(objectToAdd);                
        }

        public bool DeleteObject(WsObject objectToDelete)
        {
            //first we check if the object is in the added or modified state
            //so we can remove it from there
            _addedContainer.Remove(objectToDelete);
            _modifiedContainer.Remove(objectToDelete);

            //add it to the deleted container
            return _deletedContainer.Add(objectToDelete);
        }

        public bool UpdateObject(WsObject objectToUpdate)
        {
            bool retVal = false;
            if (_addedContainer.Contains(objectToUpdate))
            {
                //overwrite the information 
                _addedContainer.Remove(objectToUpdate);
                retVal = _addedContainer.Add(objectToUpdate);
            }
            else
            {
                //make sure is not in the deleted container
                _deletedContainer.Remove(objectToUpdate);
                retVal =_modifiedContainer.Add(objectToUpdate);
            }
            return retVal; 
        }

        public bool IsAdded(WsObject objectToVerify)
        {
            return _addedContainer.Contains(objectToVerify);
        }

        public bool IsModified(WsObject objectToVerify)
        {
            return _modifiedContainer.Contains(objectToVerify);
        }

        public bool IsDeleted(WsObject objectToVerify)
        {
            return _deletedContainer.Contains(objectToVerify);
        }

        public IEnumerable<WsObject> GetAddedObjects()
        {
            return _addedContainer.ToList<WsObject>();
        }

        public IEnumerable<WsObject> GetDeleteObjects()
        {
            return _deletedContainer.ToList<WsObject>();
        }

        public IEnumerable<WsObject> GetModifiedObjects()
        {
            return _modifiedContainer.ToList<WsObject>();
        }

        public void MarkAddedObjectsAsSaved()
        {
            _addedContainer.Clear();
        }

        public void MarkDeletedObjectsAsSaved()
        {
            _deletedContainer.Clear();
        }

        public void MarkModifiedObjectsAsSaved()
        {
            _modifiedContainer.Clear();
        }

        
    }
}
