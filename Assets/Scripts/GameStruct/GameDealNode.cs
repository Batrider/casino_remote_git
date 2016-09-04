using UnityEngine;
using System.Collections;
public class GameDealNode
{
	private int type;
	private int form;
	private double paid;
	public GameDealNode(int type,int form,double paid)
	{
		this.type = type;
		this.form = form;
		this.paid = paid;
	}
	public int TYPE
	{
		get{
			return type;
		}
		set{
			this.type = value;
		}
	}
	public int FORM
	{
		get{
			return form;
		}
		set{
			this.form = value;
		}
	}
	public double PAID
	{
		get{
			return paid;
		}
		set{
			this.paid = value;
		}
	}
}
