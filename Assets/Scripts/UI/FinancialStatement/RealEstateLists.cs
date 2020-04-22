using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealEstateLists : MonoBehaviour
{
    public GameObject incomeLiabilityListItem;
    public GameObject assetListItem;

    public Transform incomeList;
    public Transform assetList;
    public Transform liabilityList;

    private readonly List<GameObject> listItems = new List<GameObject>();

    public void ResetPlayer(Player player) {
        listItems.ForEach(listItem => Destroy(listItem));
        listItems.Clear();
        List<RealEstateCard> realEstate = player.incomeStatement.RealEstate();
        foreach (RealEstateCard realEstateCard in realEstate)
        {
            string description;
            switch (realEstateCard.propertyType)
            {
                case RealEstateType.House:
                case RealEstateType.Condo:
                    HomeCard home = (HomeCard)realEstateCard;
                    description = home.bed + "/" + home.bath + " " + (home.propertyType == RealEstateType.House ? "House" : "Condo");
                    break;
                case RealEstateType.Plex:
                    MultiCard plex = (MultiCard)realEstateCard;
                    description = plex.units == 2 ? "Duplex" : (plex.units + "-Plex");
                    break;
                case RealEstateType.Apartment:
                    MultiCard apartment = (MultiCard)realEstateCard;
                    description = apartment.units + "-Unit Apartment";
                    break;
                case RealEstateType.Business:
                default:
                    description = realEstateCard.title;
                    break;
            }

            GameObject incomeObject = Instantiate(incomeLiabilityListItem, incomeList);
            RealEstateIncomeLiabilityEntry income = incomeObject.GetComponent<RealEstateIncomeLiabilityEntry>();

            GameObject assetObject = Instantiate(assetListItem, assetList);
            RealEstateAssetEntry asset = assetObject.GetComponent<RealEstateAssetEntry>();

            GameObject liabilityObject = Instantiate(incomeLiabilityListItem, liabilityList);
            RealEstateIncomeLiabilityEntry liability = liabilityObject.GetComponent<RealEstateIncomeLiabilityEntry>();

            income.description.text = description;
            asset.description.text = description;
            liability.description.text = description;

            income.value.text = Utility.FormatMoney(realEstateCard.cashFlow);
            asset.downPayment.text = Utility.FormatMoney(realEstateCard.downPayment);
            asset.cost.text = Utility.FormatMoney(realEstateCard.cost);
            liability.value.text = Utility.FormatMoney(realEstateCard.mortgage);

            listItems.Add(incomeObject);
            listItems.Add(assetObject);
            listItems.Add(liabilityObject);

        }
    }
}
