author: dan
//Code Source: C:\Users\user\Desktop\תיקיה להרצה לחיבור קבצי קוד\Actor.java
public class Actor {
    public String id;
    private String gender;
    private int numFilms;

    public Actor(String id, String gender, int numFilms) {
        this.id = id;
        this.gender = gender;
        this.numFilms = numFilms;
    }

    public int getNumFilms() {
        return numFilms;
    }

    public int Compare(Actor other) {
        if (this.getNumFilms() == other.getNumFilms())
            return 3;
        if (this.getNumFilms() < other.getNumFilms())
            return 2;
        return 1;
    }
}



//Code Source: C:\Users\user\Desktop\תיקיה להרצה לחיבור קבצי קוד\Doc.java
public class Doc {
    private int KodDoc;
    private String IdDoc;
    private String NameDoc;
    private int WorkExperience;
    public void AddWorkExperience(){
        this.WorkExperience++;
    }
   public String getIdDoc(){
        return this.IdDoc;
   }
   public String getNameDoc(){
        return this.NameDoc;
   }
}


//Code Source: C:\Users\user\Desktop\תיקיה להרצה לחיבור קבצי קוד\List.java
public class List<T> {
}

//Code Source: C:\Users\user\Desktop\תיקיה להרצה לחיבור קבצי קוד\Tar2.java
import java.util.Scanner;
public class Tar2 {
    public static int size(Node<Int> lst) {
        int cnt = 0;
        Node<Int> pos3 = lst;
        while (pos3 != null) {
            cnt++;
            pos3 = pos3.getNext();
        }
        return cnt;
    }

    public static Node<Int> move(Node<Int> lst, int n) {
        int num = size(lst) - n;
        Node<Int> pos = lst, posNewLst = null, pos1 = null;
        while (num-1 > 0) {
            pos = pos.getNext();
            num--;
        }
        posNewLst = pos.getNext();
        pos1 = posNewLst;
        pos.setNext(null);
         while (pos1.getNext()!= null) {
            pos1 = pos1.getNext();
         }
         pos1.setNext(lst); //pos1=284
        return posNewLst;
    }

    public static void main(String[] args) {
        Scanner in=new Scanner(System.in);
        Node<Int> lst=new Node<>(5),pos=lst,pos2=lst;
        System.out.println("הקש מספר סיבובים");
        int n=in.nextInt();
        pos.setNext(new Node<>(1));
        pos=pos.getNext();
        pos.setNext(new Node<>(2));
        pos=pos.getNext();
        pos.setNext(new Node<>(8));
        pos=pos.getNext();
        pos.setNext(new Node<>(4));
        pos=pos.getNext();
         while (pos2.getNext()!=null){
            System.out.print(pos2.getValue());
            pos2=pos2.getNext();
        }
        System.out.println(pos2.getValue());
         Node<Int>newlst;
        newlst = move(lst,n);
         while (newlst!=null){
            System.out.print(newlst.getValue());
            newlst=newlst.getNext();
        }
    }
}
