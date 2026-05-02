using UnityEngine;

public class HandController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float reachDistance = 1.5f; 
    private GameObject grabbedObject; 

    void Update()
    {
        // 1. ดึงค่าจากปุ่มแบบ Raw (0 หรือ 1) เพื่อไม่ให้มีการหน่วง
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        // 2. ถ้ากดแล้วไปแต่ทางขวา เราจะติดลบที่ค่า h เพื่อบังคับให้มันไปซ้าย
        // ลองใช้สูตรนี้ครับ: -h จะทำให้ A ไปซ้าย D ไปขวา (แก้ปัญหาไปแต่ทางขวา)
        Vector3 moveDir = new Vector3(-h, 0, v); 

        if (moveDir.magnitude > 0.1f)
        {
            moveDir = moveDir.normalized;
            // ใช้ Space.Self เพื่อให้ขยับตามมุมกล้องที่คุณตั้งไว้
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);
        }

        // 3. คลิกเมาส์ซ้ายเพื่อจับ/ปล่อย
        if (Input.GetMouseButtonDown(0)) 
        {
            if (grabbedObject == null) TryPickUp();
            else DropObject();
        }
    }

    private void TryPickUp()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, reachDistance);
        foreach (var hit in hitColliders)
        {
            if (hit.CompareTag("Apple"))
            {
                grabbedObject = hit.gameObject;
                grabbedObject.transform.SetParent(this.transform);
                grabbedObject.transform.localPosition = new Vector3(0, 0, 0.5f);
                if (grabbedObject.GetComponent<Rigidbody>()) 
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                break; 
            }
        }
    }

    private void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            if (grabbedObject.GetComponent<Rigidbody>()) 
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject = null;
        }
    }
}