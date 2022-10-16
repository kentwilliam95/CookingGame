using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class TrangbulanShop : MonoBehaviour
    {
        [System.Serializable]
        public class CustomerPosition
        {
            public Customer customer;
            public Vector3 position;
        }

        public List<CustomerPosition> customerPositions;
        public CustomerPosition GetCustomerPosition()
        {
            for (int i = 0; i < customerPositions.Count; i++)
            {
                if (customerPositions[i].customer == null)
                    return customerPositions[i];
            }
            return null;
        }

        public void RemoveCustomer(Customer cust)
        {
            for (int i = customerPositions.Count - 1; i >= 0; i--)
            {
                if (customerPositions[i].customer == cust)
                {
                    customerPositions[i].customer = null;
                    break;
                }
            }
        }

        public void OnDrawGizmos()
        {
            for (int i = 0; i < customerPositions.Count; i++)
            {
                Gizmos.DrawWireSphere(customerPositions[i].position, 0.25f);
            }
        }
    }
}
