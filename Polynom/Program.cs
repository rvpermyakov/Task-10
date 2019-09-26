using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Structures
{
	public class Node
	{
		public int degree;
		public int constant;
		public Node next = null;
		public Node previous = null;

		public Node(Node previous, Node next, int degree, int constant)
		{
			this.previous = previous;
			this.next = next;
			this.degree = degree;
			this.constant = constant;
		}

		public Node() { }
	}

	public class LinkedList
	{
		public Node head = null;
		Node tail = null;

		public LinkedList() { }

		public void Add(int degree, int constant)
		{
			if (head == null) {
				head = tail = new Node(null, null, degree, constant);
			} else {
				Node toAdd = new Node(tail, null, degree, constant);
				tail.next = toAdd;
				tail = toAdd;
			}
		}

		public void Sort() {
			Node border = tail;
			while (border != head) {
				Node pointer = head;
				while (pointer != border) {
					Node temp = pointer.next;
					if (pointer.degree > temp.degree) {
						temp.previous = pointer.previous;
						if (temp.previous != null)
							temp.previous.next = temp;
						else
							head = temp;

						pointer.previous = temp;
						pointer.next = temp.next;
						temp.next = pointer;
						if (pointer.next != null)
							pointer.next.previous = pointer;
						else
							tail = pointer;

						if (temp == border)
							border = pointer;
					}
					pointer = temp;
				}
				border = border.previous;
			}
		}

		public static LinkedList operator +(LinkedList firstList, LinkedList secondList) {
			Node firstPointer = firstList.head;
			Node secondPointer = secondList.head;
			LinkedList resultList = new LinkedList();
			while (firstPointer != null || secondPointer != null) {
				if (secondPointer != null && firstPointer != null && firstPointer.degree == secondPointer.degree) {
					if(firstPointer.constant + secondPointer.constant != 0)
						resultList.Add(firstPointer.degree, firstPointer.constant + secondPointer.constant);
					firstPointer = firstPointer.next;
					secondPointer = secondPointer.next;
				} else if (secondPointer == null || firstPointer.degree < secondPointer.degree) {
					resultList.Add(firstPointer.degree, firstPointer.constant);
					firstPointer = firstPointer.next;
				} else {
					resultList.Add(secondPointer.degree, secondPointer.constant);
					secondPointer = secondPointer.next;
				}				
			}
			return resultList;
		}
	}
}

namespace Linked_List
{
	class Program
	{
		static Structures.LinkedList Read(string path) {
			StreamReader fileInput = new StreamReader(path, Encoding.Default);
			Structures.LinkedList list = new Structures.LinkedList();
			string tempStr;
			while ((tempStr = fileInput.ReadLine()) != null) {
				string[] temp = tempStr.Split(' ');
				int constant = int.Parse(temp[1]);
				if (constant != 0)
					list.Add(int.Parse(temp[0]), constant);
			}
			fileInput.Close();
			return list;
		}

		static void ToFile(Structures.LinkedList list) {
			Structures.Node pointer = list.head;
			StreamWriter fileOut = new StreamWriter("result.txt", false, Encoding.Default);
			while (pointer != null) {
				fileOut.WriteLine("{0} {1}", pointer.degree, pointer.constant);
				pointer = pointer.next;
			}
			fileOut.Close();
		}

		static void Main(string[] args)
		{
			Structures.LinkedList firstPoly = Read("firstpoly.txt");
			Structures.LinkedList secondPoly = Read("secondpoly.txt");
			firstPoly.Sort();
			secondPoly.Sort();
			ToFile(firstPoly + secondPoly);
		}
	}
}