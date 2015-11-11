<%-- 
    Document   : CartPage
    Created on : 04.11.2015, 16:20:20
    Author     : SofySmo

--%>

<%@page contentType="text/html" pageEncoding="UTF-8" isELIgnored="false"%>
<%@page language="java" import="domain.*"%>
<%@page language="java" import="java.util.ArrayList" %>
<%@ taglib uri='http://java.sun.com/jsp/jstl/fmt' prefix='fmt'%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<%@ taglib uri="http://java.sun.com/jsp/jstl/functions" prefix="fn" %>
<%@ taglib prefix="ff" uri="/WEB-INF/tlds/myfunction.tld"%>



<c:set var="order" value="${sessionScope.order}"/>
<c:choose>
<c:when test="${empty param.lang}">
    <c:set var="lang" value="ru"/>
</c:when>
<c:otherwise>
    <c:set var="lang" value="${param.lang}"/>
</c:otherwise>
</c:choose>
<fmt:setBundle var="en" basename="page_product_en" />
<fmt:setBundle basename="page_product_${lang}" />

<c:forEach var="cookies" items="${cookie}">
    <c:if test="${cookies.key == 'user'}">
        <c:set var="user" value="true"/>
</c:if>
</c:forEach>
    <c:if test="${empty user}">
           <c:redirect url="Authorization.jsp?lang=${lang}"/>
    </c:if>
<c:if test="${empty user}">
    <c:redirect url="Authorization.jsp?lang=${lang}"/>
</c:if>
<!DOCTYPE html>
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <link rel="stylesheet" type="text/css" href="style/newcss.css">
        <link rel="stylesheet" type="text/css" href="style/Cartpage.css">
        <script type="text/javascript" src="js/pageJS.js"></script>
        <script type="text/javascript" src="js/cart.js"></script>
        <script src="masonry.js"></script>
       <title><fmt:message key="title_site" /></title>
    </head>
    <body>
        <div id="main_conteiner">
           <div id="header-conteiner">
               
               <div id="hat_page">
                   <c:import url="hat_page.jsp?lang=${lang}&parant=CartPage.jsp"/>                  
              </div>
           </div>
        </div>
        <div id="big_contain">
            <div class="for_inline">
                <div id="shopfilter">
                    <h3><fmt:message key="left-menu_painting"/></h3>
                    <ul class="menu-list" id="left-navigation">
                        <li class="left-navigation-item">
                            <a class="left-link" href="http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang=${lang}"><fmt:message key="left-navigation-item_0" /></a>
                        </li>
                        <c:forEach var="i" begin="1" end="4">
                            <li class="left-navigation-item">
                                <a class="left-link" href="http://localhost:8084/Febos2/ListOfProductsPage.jsp?lang=${lang}&genre_filter=<fmt:message bundle="${en}" key="left-navigation-item_${i}"/>"><fmt:message key="left-navigation-item_${i}" /></a>
                        </li>
                        </c:forEach>
                        </ul>
                </div>
                        <div class="channel-homepage">
                            <div id="result">
                                <span><fmt:message key="sum_cost"/>: $</span>
                                <strong id="result_cost">0</strong>
                            </div>
                            
                            <h2><fmt:message key="shopping_list" /></h2>
                            <div class="list_product grid js-masonry">
                                <c:set var="sum" value="${0}"/>
                                <c:set var="i" value="${0}"/>
                                <c:choose>
                                    <c:when test="${order==null ||order.isEmpty()}">
                                        <div class="empty_message"> 
                                            <fmt:message key="empty_card_message" />
                                        </div>    
                                     </c:when>
                                    <c:otherwise>
                                        <c:forEach var="item" items="${order.getItemList()}">
                                            <fmt:parseNumber var="id" type="number" value="${item.getId()}"/>
                                                <c:set var="indexFile" value="${id/100}"/>
                                                <c:set var="indexPicture" value="${id%100}"/>
                                                <c:set var="art" value="${ff:getArt(indexFile,indexPicture)}"/>
                                                <c:set var="sum" value="${sum+art.price}"/>
                                                <div class="product grid-item choose_product" id="product_${i}">
                                                <span class="cross" onclick="refuseProduct(${i},${item.getId()},${art.price},${item.getCount()});">X</span>
                                                <br>
                                                <script type="text/javascript"> addCost(${art.price},${item.getCount()});</script>
                                                <c:import url="product.jsp?lang=${lang}&name=${art.name}&author=${art.author}&path=${art.pathartwork}&price=${art.price}"/>   
                                                <strong><fmt:message key="count"/></strong>

                                                <input type="number" id="count_${i}" min="1" max="10" step="1" class="count" value="${item.getCount()}"
                                                       onchange="onchangeCount(${i},${art.price});" >
                                                  </div>
                                                <c:set var="i" value="${i+1}"/>
                                        </c:forEach>
                                    </c:otherwise>
                                </c:choose>
                           <%-- <c:forEach var="cookies" items="${cookie}">
                                
                                <c:if test="${fn:indexOf(cookies.key,'#') eq 0}">
                                    
                                    <c:catch>
                                        <fmt:parseNumber var="id" type="number" value="${fn:substring(cookies.key,1,fn:length(cookies.key))}"/>
                                    </c:catch>
                                    <c:if test="${catchException eq null}">
                                        
                                        <c:set var="indexFile" value="${id/100}"/>
                                        <c:set var="indexPicture" value="${id%100}"/>
                                        <c:set var="art" value="${ff:getArt(indexFile,indexPicture)}"/>
                                        <c:set var="sum" value="${sum+art.price}"/>
                                        
                                        
                                        <div class="product grid-item choose_product" id="product_${i}">
                                            <span id="cookie_${i}" style="display:none"><c:out value="${cookies.key}"/></span>
                                            <span id="oldcount_${i}" style="display:none">${cookies.value.value}</span>
                                            <span class="cross" onclick="refuseToBuy(${i},${art.price},${cookies.value.value});">X</span>
                                            <br>
                                            <script type="text/javascript"> addCost(${art.price},${cookies.value.value});</script>
                                            <c:import url="product.jsp?lang=${lang}&name=${art.name}&author=${art.author}&path=${art.pathartwork}&price=${art.price}"/>   
                                            <strong><fmt:message key="count"/></strong>
                                            
                                            <input type="number" id="count_${i}" min="1" max="10" step="1" class="count" value="${cookies.value.value}"
                                                   onchange="onchangeCount(${i},${art.price});" >
                                              </div>
                                            <c:set var="i" value="${i+1}"/>
                                    </c:if>
                                    <c:set var="catchException" value="${null}"/> 
                                </c:if>       
                            </c:forEach>     --%>
                            </div>
                           
                        </div>
                             
                </div>
           </div>
           <div class="endpage">
                    <p>&copy;2015 Фебос.</p>
           </div>
        </div>
    </body>
</html>
