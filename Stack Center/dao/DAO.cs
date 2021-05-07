using System.Collections.Generic;

namespace Stack_Center.dao
{
    internal interface IDAO<T>
    {
        public void addElement(T obj);

        public T getElement(int id);

        public void removeElement(int id);

        public void updateElement(T obj, int id);

        public List<T> getAll();
    }
}