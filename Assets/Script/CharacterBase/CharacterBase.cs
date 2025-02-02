using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum ColorType
{
    red = 0, green = 1, blue = 2, yellow = 3, None
};

public class CharacterBase : MonoBehaviour
{
    public ColorType color; 
    public Vector3 velocity;
    public SkinnedMeshRenderer skinnedMeshRenderer; 

    public List<Material> listMau; 
    public List<GameObject> playerBrick = new List<GameObject>();
    [System.NonSerialized]
    public List<Brick> listBrickHiden = new List<Brick>();

    public GameObject brickPrefab; 
    public Transform startBrick;

    StairsCheck stairsCheck; 
    public float checkStairDistance; 
    public LayerMask lmStair; 
    public Transform checkStair; 
    public GameObject stairPrefab; 

    SpoinBrick spoinBrick; 
    public float haibrick;
    public bool hasTriggered = true;

    public Rigidbody rb;
    //
    public Animator anim {  get; private set; }
    // Khởi tạo màu sắc cho nhân vật
    protected virtual void Awake()
    {
        ChangeColor();
    }

    // Lấy các đối tượng cần thiết khi bắt đầu
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        stairsCheck = FindObjectOfType<StairsCheck>();
        spoinBrick = FindObjectOfType<SpoinBrick>();
        
    }

    // Cập nhật vị trí và kiểm tra cầu thang trong mỗi khung hình
    protected virtual void Update()
    {
    }

    // Thêm một viên gạch mới vào danh sách
    public virtual void AddBrick(Brick brick)
    {
        if (brickPrefab == null || brick == null)
        {
            Debug.LogError("Brick Prefab is not assigned!");
            return;
        }
        haibrick += 0.21f ;
        listBrickHiden.Add(brick);
        brick.gameObject.SetActive(false);
        if (listBrickHiden.Count > playerBrick.Count)
        {
        GameObject newBrick = Instantiate(brickPrefab);
        newBrick.transform.SetParent(transform, true); 
        newBrick.transform.localRotation = Quaternion.Euler(0, 0, 0); 

        newBrick.transform.position = new Vector3(startBrick.position.x, startBrick.position.y + haibrick, startBrick.position.z);

        Brick brickComponent = newBrick.GetComponent<Brick>(); 
        if (brickComponent != null)
        {
            brickComponent.color = color; 
            brickComponent.ChangeColor();  
        }
        playerBrick.Add(newBrick);
        }
        else
        {
            playerBrick[listBrickHiden.Count -1].gameObject.SetActive(true);
        }
    }

    // Xóa viên gạch khỏi danh sách và xử lý việc xuất hiện lại viên gạch
    public virtual void RemoveBrick(Brick brick)
    {

            brick.gameObject.SetActive(true);
            listBrickHiden.Remove(brick);
            haibrick -= 0.21f;
            int index = listBrickHiden.Count;
            GameObject abang = playerBrick[index].gameObject;
            abang.SetActive(false);
    }

    public virtual void HideAllBrick(Brick brick)
    {
        brick.gameObject.SetActive(true);
    }

    // Kiểm tra xem có cầu thang nào dưới chân nhân vật không



    public virtual void ClearBrick()
    {
            foreach (var brick in playerBrick)
            {
                if (brick != null)
                {
                    brick.gameObject.SetActive(false);
                }
            }
    }

    // Lấy vật liệu dựa trên màu sắc
    public Material ColorType(ColorType color)
    {
        return listMau[(int)color]; // Trả về vật liệu tương ứng với màu
    }

    // Đổi màu sắc cho nhân vật
    public void ChangeColor()
    {
        Material[] mat = skinnedMeshRenderer.materials;
        mat[0] = ColorType(color); 
        skinnedMeshRenderer.materials = mat; 
    }
}
