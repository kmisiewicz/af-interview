namespace AFSInterview.Items
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
	using UnityEngine.Pool;

    public class ItemsManager : MonoBehaviour
	{
		[SerializeField] private InventoryController inventoryController;
		[SerializeField] private int itemSellMaxValue;
		[SerializeField] private Transform itemSpawnParent;
		[SerializeField] private List<GameObject> itemPrefabs;
		[SerializeField] private BoxCollider itemSpawnArea;
		[SerializeField] private float itemSpawnInterval;
		[SerializeField] private int itemPoolDefaultCapacity = 10;
		[SerializeField] private int itemPoolMaxSize = 30;
		[SerializeField] private int maxSpawnedItems = 50;

		private Camera mainCamera;
		private LayerMask itemsLayerMask;
		private ObjectPool<GameObject> itemPool;

		private void Start ()
		{
			mainCamera = Camera.main;
			itemsLayerMask = LayerMask.GetMask("Item");

			InitializeItemPool();
			StartCoroutine(SpawnItems());
        }
		
		private void Update()
		{			
			if (Input.GetMouseButtonDown(0))
				TryPickUpItem();
			
			if (Input.GetKeyDown(KeyCode.Space))
				inventoryController.SellAllItemsUpToValue(itemSellMaxValue);
		}

		private IEnumerator SpawnItems()
		{
			while (true)
			{
				if (itemPool.CountActive < maxSpawnedItems)
					itemPool.Get();
				yield return new WaitForSeconds(itemSpawnInterval);
			}
		}

        private void TryPickUpItem()
		{
			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out var hit, 100f, itemsLayerMask) || !hit.collider.TryGetComponent(out IItemHolder itemHolder))
				return;
			
			var item = itemHolder.GetItem(false);
			itemPool.Release(hit.collider.gameObject);
            inventoryController.AddItem(item);

            Debug.Log($"Picked up {item.Name} with value of {item.Value} and now have {inventoryController.ItemsCount} items");
		}

		private Vector3 GetRandomSpawnPosition()
		{
            var spawnAreaBounds = itemSpawnArea.bounds;
			return new Vector3(
                Random.Range(spawnAreaBounds.min.x, spawnAreaBounds.max.x),
                0f,
                Random.Range(spawnAreaBounds.min.z, spawnAreaBounds.max.z)
            );
        }

		private GameObject GetRandomPrefabToSpawn()
		{
            return itemPrefabs[Random.Range(0, itemPrefabs.Count)];
		}

		private void InitializeItemPool()
		{
			itemPool = new ObjectPool<GameObject>(CreatePooledItem, OnGetFromPool, OnReleaseToPool, OnDestroyPooledItem, 
				true, itemPoolDefaultCapacity, itemPoolMaxSize);
		}

		private GameObject CreatePooledItem()
		{
			return Instantiate(GetRandomPrefabToSpawn(), GetRandomSpawnPosition(), Quaternion.identity, itemSpawnParent);
        }

		private void OnGetFromPool(GameObject go)
		{
			go.transform.position = GetRandomSpawnPosition();
			go.SetActive(true);
		}

		private void OnReleaseToPool(GameObject go)
		{
			go.SetActive(false);
			Vector3 position = go.transform.position;
			position.y = -10f;
			go.transform.position = position;
		}

		private void OnDestroyPooledItem(GameObject go)
		{
			Destroy(go);
		}
	}
}